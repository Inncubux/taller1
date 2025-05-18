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

/// <summary>
/// Controller responsible for user authentication and registration.
/// Provides endpoints for user registration, login, and logout.
/// </summary>
namespace ECommerce.src.Controllers
{
    public class AuthController(ILogger<AuthController> logger, UserManager<User> userManager, ITokenService tokenService) : BaseApiController
    {
        private readonly ILogger<AuthController> _logger = logger;
        private readonly UserManager<User> _userManager = userManager;
        private readonly ITokenService _tokenService = tokenService;

        /// <summary>
        /// Registers a new user.
        /// Validates the input, checks password confirmation, age, and creates the user with the "User" role.
        /// Returns a JWT token upon successful registration.
        /// </summary>
        /// <param name="newUser">Registration data transfer object</param>
        /// <returns>ApiResponse with AuthUserDto and JWT token</returns>
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto newUser)
        {
            try
            {
                // Validate model state
                if (!ModelState.IsValid)
                {
                    return BadRequest(new ApiResponse<string>(false, "Invalid data.", null, ModelState.Values.SelectMany(x => x.Errors).Select(x => x.ErrorMessage).ToList()));
                }

                // Map DTO to User entity
                var user = UserMapper.RegisterToUser(newUser);

                // Check password fields
                if (string.IsNullOrEmpty(newUser.Password) || string.IsNullOrWhiteSpace(newUser.ConfirmPassword))
                {
                    return BadRequest(new ApiResponse<string>(false, "Password cannot be empty."));
                }

                // Check password confirmation
                if (newUser.Password != newUser.ConfirmPassword)
                {
                    return BadRequest(new ApiResponse<string>(false, "Passwords do not match."));
                }

                // Check birth date is not in the future
                if (newUser.BirthDate > DateTime.UtcNow)
                {
                    return BadRequest(new ApiResponse<string>(false, "Birth date cannot be in the future."));
                }

                // Check user is at least 18 years old
                var today = DateTime.UtcNow;
                var age = today.Year - newUser.BirthDate.Year;
                if (newUser.BirthDate.Date > today.AddYears(-age).Date)
                {
                    age--;
                }

                if (age < 18)
                {
                    return BadRequest(new ApiResponse<string>(false, "Debes tener al menos 18 años para registrarte."));
                }

                // Create user
                var createUser = await _userManager.CreateAsync(user, newUser.Password);

                if (!createUser.Succeeded)
                {
                    return BadRequest(new ApiResponse<string>(false, "Error al crear el usuario.", null, createUser.Errors.Select(x => x.Description).ToList()));

                }

                // Assign "User" role
                var roleUser = await _userManager.AddToRoleAsync(user, "User");
                if (!roleUser.Succeeded)
                {
                    return BadRequest(new ApiResponse<string>(false, "Error al asignar el rol.", null, roleUser.Errors.Select(x => x.Description).ToList()));
                }

                // Get user role
                var role = await _userManager.GetRolesAsync(user);
                var roleName = role.FirstOrDefault() ?? "User";

                // Generate JWT token
                var token = _tokenService.GenerateToken(user, roleName);
                var userDto = UserMapper.UserToAuthUserDto(user, token);

                return Ok(new ApiResponse<AuthUserDto>(true, "Usuario creado correctamente.", userDto));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<string>(false, "Error interno del servidor.", null, new List<string> { ex.Message }));
            }
        }

        /// <summary>
        /// Authenticates a user and returns a JWT token.
        /// Checks credentials, user status, and session.
        /// </summary>
        /// <param name="loginDto">Login data transfer object</param>
        /// <returns>ApiResponse with JWT token</returns>
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
            try
            {
                // Validate model state
                if (!ModelState.IsValid)
                {
                    return BadRequest(new ApiResponse<string>(false, "Datos inválidos", null, ModelState.Values.SelectMany(x => x.Errors).Select(x => x.ErrorMessage).ToList()));
                }

                // Find user by email
                var user = await _userManager.FindByEmailAsync(loginDto.Email);
                if (user == null)
                {
                    return Unauthorized(new ApiResponse<string>(false, "Correo o contraseña incorrectos."));
                }

                // Check if user is enabled
                if (!user.Status)
                {
                    return Unauthorized(new ApiResponse<string>(false, "Tu cuenta se encuentra inhabilitada."));
                }

                // Check password
                var result = await _userManager.CheckPasswordAsync(user, loginDto.Password);
                if (!result)
                {
                    return Unauthorized(new ApiResponse<string>(false, "Correo o contraseña incorrectos."));
                }

                // Check for active session
                var activeSessionUserId = HttpContext.Session.GetString("UserId");
                if (activeSessionUserId != null)
                {
                    return BadRequest(new ApiResponse<string>(false, "Ya tienes una sesión activa."));
                }

                // Update last login date
                user.LastLogin = DateTime.UtcNow;
                await _userManager.UpdateAsync(user);

                // Get user role
                var role = await _userManager.GetRolesAsync(user);
                var roleName = role.FirstOrDefault() ?? "User";

                // Generate JWT token
                var token = _tokenService.GenerateToken(user, roleName);
                var userDto = UserMapper.UserToAuthUserDto(user, token);

                // Store user ID in session
                HttpContext.Session.SetString("UserId", user.Id);


                return Ok(new ApiResponse<string>(true, "Inicio de sesión exitoso", token));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<string>(false, "Error interno del servidor", null, new List<string> { ex.Message }));
            }
        }

        /// <summary>
        /// Logs out the current user by clearing the session.
        /// </summary>
        /// <returns>ApiResponse indicating logout status</returns>
        [HttpPost("logout")]
        public IActionResult Logout()
        {
            // Check if there is an active session
            if (HttpContext.Session.GetString("UserId") == null)
            {
                return BadRequest(new ApiResponse<string>(false, "No hay ninguna sesión activa."));
            }
            // Clear session
            HttpContext.Session.Clear();
            return Ok(new ApiResponse<string>(true, "Sesión cerrada correctamente."));
        }
    }
}