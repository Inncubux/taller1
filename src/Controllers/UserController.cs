using System;
using System.Collections;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

using Azure.Identity;

using Bogus.Extensions.UnitedKingdom;

using ECommerce.src.Data;
using ECommerce.src.Dto.User;
using ECommerce.src.Extensions;
using ECommerce.src.Helpers;
using ECommerce.src.Helpers.RequestHelpers;
using ECommerce.src.Mappers;
using ECommerce.src.Models;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.EntityFrameworkCore;

using taller1.src.Models;

/// <summary>
/// Controller responsible for user management operations such as retrieving, updating, changing status, and changing password for users.
/// </summary>
namespace ECommerce.src.Controllers
{
    public class UserController(ILogger<UserController> logger, UnitOfWork unitOfWork) : BaseApiController
    {
        private readonly ILogger<UserController> _logger = logger;
        private readonly UnitOfWork _unitOfWork = unitOfWork;

        /// <summary>
        /// Retrieves a paginated list of users with optional filters for status, search term, and registration date range.
        /// </summary>
        /// <param name="userParams">Parameters for filtering and pagination</param>
        /// <returns>ApiResponse with a list of UserDto</returns>
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<ActionResult<ApiResponse<IEnumerable<UserDto>>>> GetAllUsers([FromQuery] UserParams userParams)
        {
            var query = _unitOfWork.UserRepository.GetUsersQueryable();

            // Apply filters based on userParams
            if (userParams.Status.HasValue)
            {
                query = query.Where(x => x.Status == userParams.Status.Value);
            }
            // Filter by search term
            if (!string.IsNullOrWhiteSpace(userParams.SearchTerm))
            {
                var term = userParams.SearchTerm.ToLower();
                query = query.Where(x => x.FirstName.Contains(term.ToLower()) ||
                                    x.LastName.Contains(term.ToLower()) ||
                                    (x.Email != null && x.Email.Contains(term.ToLower())));
            }
            // Filter by registration date range
            if (userParams.RegistrationDateTo < userParams.RegistrationDateFrom)
            {
                return BadRequest(new ApiResponse<string>(false, "La fecha de registro inicial no puede ser mayor a la fecha de registro final."));
            }
            // Filter by registration date range
            if (userParams.RegistrationDateFrom.HasValue)
            {
                query = query.Where(x => x.RegistrationDate >= userParams.RegistrationDateFrom.Value);
            }
            if (userParams.RegistrationDateTo.HasValue)
            {
                query = query.Where(x => x.RegistrationDate <= userParams.RegistrationDateTo.Value);
            }

            var total = await query.CountAsync();
            // Check if there are any users
            var users = await query
                .OrderByDescending(x => x.RegistrationDate)
                .Skip((userParams.PageNumber - 1) * userParams.PageSize)
                .Take(userParams.PageSize)
                .ToListAsync();

            var userDtos = users.Select(x => UserMapper.UserToUserDto(x)).ToList();
            // Add pagination header
            Response.AddPaginationHeader(new PaginationMetaData
            {
                CurrentPage = userParams.PageNumber,
                PageSize = userParams.PageSize,
                TotalCount = total,
                TotalPages = (int)Math.Ceiling((double)total / userParams.PageSize)
            });

            return Ok(new ApiResponse<IEnumerable<UserDto>>(true, "Usuarios obtenidos correctamente.", userDtos));
        }


        /// <summary>
        /// Retrieves a user by their unique identifier.
        /// </summary>
        /// <param name="id">User ID</param>
        /// <returns>ApiResponse with UserDto</returns>
        [Authorize(Roles = "Admin")]
        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse<UserDto>>> GetUserById(string id)
        {
            // Validate the user ID format
            var user = await _unitOfWork.UserRepository.GetUserByIdAsync(id);

            if (user == null)
            {
                return NotFound(new ApiResponse<string>(false, "Usuario no encontrado."));
            }

            var userDto = UserMapper.UserToUserDto(user);

            return Ok(new ApiResponse<UserDto>(true, "Usuario obtenido correctamente.", userDto));
        }

        /// <summary>
        /// Updates the status (active/inactive) of a user by their ID.
        /// </summary>
        /// <param name="id">User ID</param>
        /// <param name="userStatusDto">DTO containing the new status and optional deactivation reason</param>
        /// <returns>ApiResponse with operation result</returns>
        [Authorize(Roles = "Admin")]
        [HttpPut("{id}/status")]

        public async Task<ActionResult<ApiResponse<string>>> UpdateUserStatus(string id, [FromBody] UserStatusDto userStatusDto)
        {
            var user = await _unitOfWork.UserRepository.GetUserByIdAsync(id);

            if (user == null)
            {
                return NotFound(new ApiResponse<string>(false, "Usuario no encontrado."));
            }
            // Check if the user is already deactivated
            if (userStatusDto.Status == false)
            {
                user.DeactivationReason = userStatusDto.Reason;
                user.Status = false;
            }
            else if (userStatusDto.Status == true)
            {
                user.DeactivationReason = null;
                user.Status = true;
            }
            else
            {
                return BadRequest(new ApiResponse<string>(false, "El estado del usuario no es válido."));
            }
            await _unitOfWork.UserRepository.UpdateUserAsync(user);
            await _unitOfWork.SaveChangesAsync();

            return Ok(new ApiResponse<string>(true, "Estado del usuario actualizado correctamente."));
        }

        /// <summary>
        /// Updates the profile information of the currently authenticated user.
        /// Only updates fields provided in the request; others remain unchanged.
        /// </summary>
        /// <param name="updateUserDto">DTO containing updated user information</param>
        /// <returns>ApiResponse with updated UserDto</returns>
        [Authorize]
        [HttpPut("Update")]
        public async Task<ActionResult<ApiResponse<string>>> UpdateUser([FromBody] UpdateUserDto updateUserDto)
        {
            var userEmail = User.FindFirst(ClaimTypes.Email)?.Value;
            if (string.IsNullOrEmpty(userEmail))
            {
                return Unauthorized(new ApiResponse<string>(false, "No se pudo indetificar el usuario."));
            }

            var user = await _unitOfWork.UserRepository.GetUserByEmailAsync(userEmail);
            if (user == null)
            {
                HttpContext.Session.Clear();
                return Unauthorized(new ApiResponse<string>(false, "Inicie sesión nuevamente."));
            }
            // Validate if user email already exists
            var email = await _unitOfWork.UserRepository.GetUserByEmailAsync(updateUserDto.Email);
            if (email != null && email.Id != user.Id)
            {
                return BadRequest(new ApiResponse<string>(false, "El correo electrónico ya está en uso."));
            }

            if (user.Id != HttpContext.Session.GetString("UserId"))
            {
                return Unauthorized(new ApiResponse<string>(false, "No tienes permiso para actualizar este usuario."));
            }
            // Change user properties only if they are provided in the request
            user.FirstName = updateUserDto.FirstName ?? user.FirstName;
            user.LastName = updateUserDto.LastName ?? user.LastName;
            user.Email = updateUserDto.Email ?? user.Email;
            user.PhoneNumber = updateUserDto.Phone ?? user.PhoneNumber;
            user.BirthDate = updateUserDto.BirthDate ?? user.BirthDate;
            user.Address = new Address
            {
                Street = updateUserDto.Street != null ? updateUserDto.Street : user.Address.Street,
                Number = updateUserDto.Number != null ? updateUserDto.Number : user.Address.Number,
                Commune = updateUserDto.Commune != null ? updateUserDto.Commune : user.Address.Commune,
                Region = updateUserDto.Region != null ? updateUserDto.Region : user.Address.Region,
                PostalCode = updateUserDto.PostalCode != null ? updateUserDto.PostalCode : user.Address.PostalCode
            };
            // Update user in the database
            await _unitOfWork.UserRepository.UpdateUserAsync(user);
            await _unitOfWork.SaveChangesAsync();

            var userDto = UserMapper.UserToUserDto(user);

            return Ok(new ApiResponse<UserDto>(true, "Usuario actualizado correctamente.", userDto));
        }

        /// <summary>
        /// Changes the password of the currently authenticated user.
        /// Validates the current password, new password, and confirmation.
        /// </summary>
        /// <param name="changePasswordDto">DTO containing current password, new password, and confirmation</param>
        /// <returns>ApiResponse with operation result</returns>
        [Authorize]
        [HttpPut("ChangePassword")]

        public async Task<ActionResult<ApiResponse<string>>> ChangePassword([FromBody] ChangePasswordDto changePasswordDto)
        {
            var userEmail = User.FindFirst(ClaimTypes.Email)?.Value;
            if (string.IsNullOrEmpty(userEmail))
            {
                return Unauthorized(new ApiResponse<string>(false, "No se pudo indetificar el usuario."));
            }

            var user = await _unitOfWork.UserRepository.GetUserByEmailAsync(userEmail);
            if (user == null)
            {
                return NotFound(new ApiResponse<string>(false, "Usuario no encontrado."));
            }

            if (user.Id != HttpContext.Session.GetString("UserId"))
            {
                return Unauthorized(new ApiResponse<string>(false, "No tienes permiso para actualizar este usuario."));
            }
            // Validate if the current password is correct
            if (changePasswordDto.NewPassword != changePasswordDto.ConfirmPassword)
            {
                return BadRequest(new ApiResponse<string>(false, "Las contraseñas no coinciden."));
            }
            if (changePasswordDto.Password == changePasswordDto.NewPassword)
            {
                return BadRequest(new ApiResponse<string>(false, "La nueva contraseña no puede ser igual a la contraseña actual."));
            }

            if (string.IsNullOrEmpty(changePasswordDto.Password) || string.IsNullOrWhiteSpace(changePasswordDto.Password))
            {
                return BadRequest(new ApiResponse<string>(false, "La contraseña no puede estar vacía."));
            }

            if (changePasswordDto.Password != user.Password)
            {

                return BadRequest(new ApiResponse<string>(false, "La contraseña actual no es correcta."));
            }

            var newPassword = await _unitOfWork.UserRepository.ChangePasswordAsync(user, changePasswordDto.NewPassword);
            if (!newPassword.Succeeded)
            {
                return BadRequest(new ApiResponse<string>(false, "Error al cambiar la contraseña.", null, newPassword.Errors.Select(x => x.Description).ToList()));
            }
            user.Password = changePasswordDto.NewPassword;
            // Update user in the database
            await _unitOfWork.UserRepository.UpdateUserAsync(user);

            return Ok(new ApiResponse<string>(true, "Contraseña cambiada correctamente."));
        }
    }

}