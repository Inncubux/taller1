using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ECommerce.src.Dto.User;
using ECommerce.src.Helpers;
using ECommerce.src.Interfaces;
using ECommerce.src.Mappers;
using ECommerce.src.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;

namespace ECommerce.src.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController(ILogger<AuthController> logger, UserManager<User> userManager, ITokenService tokenService) : BaseApiController
    {
        private readonly ILogger<AuthController> _logger = logger;
        private readonly UserManager<User> _userManager = userManager;
        private readonly ITokenService _tokenService = tokenService;

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto newUser)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    _logger.LogWarning("Registro fallido: datos inválidos para {Email}", newUser.Email);
                    return BadRequest(new ApiResponse<string>(false, "Datos inválidos.", null, ModelState.Values.SelectMany(x => x.Errors).Select(x => x.ErrorMessage).ToList()));
                }

                if (string.IsNullOrWhiteSpace(newUser.Password) || string.IsNullOrWhiteSpace(newUser.ConfirmPassword))
                {
                    _logger.LogWarning("Registro fallido: contraseña vacía para {Email}", newUser.Email);
                    return BadRequest(new ApiResponse<string>(false, "La contraseña no puede estar vacía."));
                }

                if (newUser.Password != newUser.ConfirmPassword)
                {
                    _logger.LogWarning("Registro fallido: contraseñas no coinciden para {Email}", newUser.Email);
                    return BadRequest(new ApiResponse<string>(false, "Las contraseñas no coinciden."));
                }

                if (newUser.BirthDate > DateTime.UtcNow)
                {
                    _logger.LogWarning("Registro fallido: fecha de nacimiento futura para {Email}", newUser.Email);
                    return BadRequest(new ApiResponse<string>(false, "La fecha de nacimiento no puede ser mayor a la fecha actual."));
                }

                var today = DateTime.UtcNow;
                var age = today.Year - newUser.BirthDate.Year;
                if (newUser.BirthDate.Date > today.AddYears(-age).Date) age--;

                if (age < 18)
                {
                    _logger.LogWarning("Registro fallido: usuario menor de edad ({Age}) para {Email}", age, newUser.Email);
                    return BadRequest(new ApiResponse<string>(false, "Debes tener al menos 18 años para registrarte."));
                }

                var user = UserMapper.RegisterToUser(newUser);
                var createUser = await _userManager.CreateAsync(user, newUser.Password);

                if (!createUser.Succeeded)
                {
                    _logger.LogWarning("Registro fallido: errores al crear el usuario {Email}. Errores: {Errors}", newUser.Email, string.Join("; ", createUser.Errors.Select(e => e.Description)));
                    return BadRequest(new ApiResponse<string>(false, "Error al crear el usuario.", null, createUser.Errors.Select(x => x.Description).ToList()));
                }

                var roleUser = await _userManager.AddToRoleAsync(user, "User");
                if (!roleUser.Succeeded)
                {
                    _logger.LogWarning("Registro fallido: error al asignar rol al usuario {Email}", newUser.Email);
                    return BadRequest(new ApiResponse<string>(false, "Error al asignar el rol.", null, roleUser.Errors.Select(x => x.Description).ToList()));
                }

                var role = await _userManager.GetRolesAsync(user);
                var token = _tokenService.GenerateToken(user, role.FirstOrDefault() ?? "User");
                var userDto = UserMapper.UserToAuthUserDto(user, token);

                _logger.LogInformation("Usuario registrado exitosamente: {Email}, ID: {UserId}", user.Email, user.Id);
                return Ok(new ApiResponse<AuthUserDto>(true, "Usuario creado correctamente.", userDto));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error inesperado durante el registro para {Email}", newUser.Email);
                return StatusCode(500, new ApiResponse<string>(false, "Error interno del servidor.", null, new List<string> { ex.Message }));
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    _logger.LogWarning("Login fallido: datos inválidos para {Email}", loginDto.Email);
                    return BadRequest(new ApiResponse<string>(false, "Datos inválidos", null, ModelState.Values.SelectMany(x => x.Errors).Select(x => x.ErrorMessage).ToList()));
                }

                var user = await _userManager.FindByEmailAsync(loginDto.Email);
                if (user == null)
                {
                    _logger.LogWarning("Login fallido: usuario no encontrado para {Email}", loginDto.Email);
                    return Unauthorized(new ApiResponse<string>(false, "Correo o contraseña incorrectos."));
                }

                if (!user.Status)
                {
                    _logger.LogWarning("Login fallido: cuenta inhabilitada para {Email}", loginDto.Email);
                    return Unauthorized(new ApiResponse<string>(false, "Tu cuenta se encuentra inhabilitada."));
                }

                var result = await _userManager.CheckPasswordAsync(user, loginDto.Password);
                if (!result)
                {
                    _logger.LogWarning("Login fallido: contraseña incorrecta para {Email}", loginDto.Email);
                    return Unauthorized(new ApiResponse<string>(false, "Correo o contraseña incorrectos."));
                }

                var activeSessionUserId = HttpContext.Session.GetString("UserId");
                if (activeSessionUserId != null)
                {
                    _logger.LogWarning("Login fallido: sesión ya activa para {Email}", loginDto.Email);
                    return BadRequest(new ApiResponse<string>(false, "Ya tienes una sesión activa."));
                }

                user.LastLogin = DateTime.UtcNow;
                await _userManager.UpdateAsync(user);

                var role = await _userManager.GetRolesAsync(user);
                var token = _tokenService.GenerateToken(user, role.FirstOrDefault() ?? "User");

                HttpContext.Session.SetString("UserId", user.Id);

                _logger.LogInformation("Inicio de sesión exitoso: {Email}", loginDto.Email);
                return Ok(new ApiResponse<string>(true, "Inicio de sesión exitoso", token));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error inesperado durante el login para {Email}", loginDto.Email);
                return StatusCode(500, new ApiResponse<string>(false, "Error interno del servidor", null, new List<string> { ex.Message }));
            }
        }

        [HttpPost("logout")]
        public IActionResult Logout()
        {
            var userId = HttpContext.Session.GetString("UserId");

            if (userId == null)
            {
                _logger.LogWarning("Logout fallido: no hay sesión activa");
                return BadRequest(new ApiResponse<string>(false, "No hay ninguna sesión activa."));
            }

            HttpContext.Session.Clear();
            _logger.LogInformation("Sesión cerrada correctamente para el usuario con ID: {UserId}", userId);
            return Ok(new ApiResponse<string>(true, "Sesión cerrada correctamente."));
        }
    }
}
