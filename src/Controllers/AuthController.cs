using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using ECommerce.src.Dto;
using ECommerce.src.Helpers;
using ECommerce.src.Interfaces;
using ECommerce.src.Mappers;
using ECommerce.src.Models;

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.src.Controllers
{
    public class AuthController(ILogger<AuthController> logger, UserManager<User> userManager, ITokenService tokenService) : BaseApiController
    {
        private readonly ILogger<AuthController> _logger = logger;
        private readonly UserManager<User> _userManager = userManager;
        private readonly ITokenService _tokenService = tokenService;

        [Route("register")]
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto newUser)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(new ApiResponse<string>(false, "Datos inválidos.", null, ModelState.Values.SelectMany(x => x.Errors).Select(x => x.ErrorMessage).ToList()));
                }

                var user = UserMapper.RegisterToUser(newUser);
                if (string.IsNullOrEmpty(newUser.Password) || string.IsNullOrWhiteSpace(newUser.ConfirmPassword))
                {
                    return BadRequest(new ApiResponse<string>(false, "La contraseña no puede estar vacía."));
                }

                var createUser = await _userManager.CreateAsync(user, newUser.Password);

                if (!createUser.Succeeded)
                {
                    return BadRequest(new ApiResponse<string>(false, "Error al crear el usuario.", null, createUser.Errors.Select(x => x.Description).ToList()));

                }

                var roleUser = await _userManager.AddToRoleAsync(user, "User");
                if (!roleUser.Succeeded)
                {
                    return BadRequest(new ApiResponse<string>(false, "Error al asignar el rol.", null, roleUser.Errors.Select(x => x.Description).ToList()));
                }

                var role = await _userManager.GetRolesAsync(user);
                var roleName = role.FirstOrDefault() ?? "User";

                var token = _tokenService.GenerateToken(user, roleName);
                var userDto = UserMapper.UserToAuthUserDto(user, token);

                return Ok(new ApiResponse<AuthUserDto>(true, "Usuario creado correctamente.", userDto));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<string>(false, "Error interno del servidor.", null, new List<string> { ex.Message }));
            }
        }
        [Route("login")]
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(new ApiResponse<string>(false, "Datos inválidos", null, ModelState.Values.SelectMany(x => x.Errors).Select(x => x.ErrorMessage).ToList()));
                }

                var user = await _userManager.FindByEmailAsync(loginDto.Email);
                if (user == null)
                {
                    return Unauthorized(new ApiResponse<string>(false, "Correo o contraseña incorrectos."));
                }

                if (!user.Status)
                {
                    return Unauthorized(new ApiResponse<string>(false, "Tu cuenta se encuentra inhabilitada."));
                }

                var result = await _userManager.CheckPasswordAsync(user, loginDto.Password);
                if (!result)
                {
                    return Unauthorized(new ApiResponse<string>(false, "Correo o contraseña incorrectos."));
                }


                user.LastLogin = DateTime.UtcNow;
                await _userManager.UpdateAsync(user);

                var role = await _userManager.GetRolesAsync(user);
                var roleName = role.FirstOrDefault() ?? "User";

                var token = _tokenService.GenerateToken(user, roleName);
                var userDto = UserMapper.UserToAuthUserDto(user, token);

                return Ok(new ApiResponse<AuthUserDto>(true, "Inicio de sesión exitoso", userDto));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<string>(false, "Error interno del servidor", null, new List<string> { ex.Message }));
            }
        }
    }
}