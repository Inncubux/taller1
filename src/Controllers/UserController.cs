using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Azure.Identity;

using Bogus.Extensions.UnitedKingdom;

using ECommerce.src.Data;
using ECommerce.src.Dto;
using ECommerce.src.Extensions;
using ECommerce.src.Helpers;
using ECommerce.src.Helpers.RequestHelpers;
using ECommerce.src.Mappers;
using ECommerce.src.Models;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.src.Controllers
{
    public class UserController(ILogger<UserController> logger, UnitOfWork unitOfWork) : BaseApiController
    {
        private readonly ILogger<UserController> _logger = logger;
        private readonly UnitOfWork _unitOfWork = unitOfWork;

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<ActionResult<ApiResponse<IEnumerable<UserDto>>>> GetAllUsers([FromQuery] UserParams userParams)
        {
            var query = _unitOfWork.UserRepository.GetUsersQueryable();

            if (userParams.Status.HasValue)
            {
                query = query.Where(x => x.Status == userParams.Status.Value);
            }

            if (!string.IsNullOrWhiteSpace(userParams.SearchTerm))
            {
                var term = userParams.SearchTerm.ToLower();
                query = query.Where(x => x.FirstName.Contains(term.ToLower()) ||
                                    x.LastName.Contains(term.ToLower()) ||
                                    (x.Email != null && x.Email.Contains(term.ToLower())));
            }

            if(userParams.RegistrationDateTo<userParams.RegistrationDateFrom)
            {
                return BadRequest(new ApiResponse<string>(false, "La fecha de registro inicial no puede ser mayor a la fecha de registro final."));
            }

            if (userParams.RegistrationDateFrom.HasValue)
            {
                query = query.Where(x => x.RegistrationDate >= userParams.RegistrationDateFrom.Value);
            }
            if (userParams.RegistrationDateTo.HasValue)
            {
                query = query.Where(x => x.RegistrationDate <= userParams.RegistrationDateTo.Value);
            }

            var total = await query.CountAsync();

            var users = await query
                .OrderByDescending(x => x.RegistrationDate)
                .Skip((userParams.PageNumber - 1) * userParams.PageSize)
                .Take(userParams.PageSize)
                .ToListAsync();

            var userDtos = users.Select(x => UserMapper.UserToUserDto(x)).ToList();

            Response.AddPaginationHeader(new PaginationMetaData
            {
                CurrentPage = userParams.PageNumber,
                PageSize = userParams.PageSize,
                TotalCount = total,
                TotalPages = (int)Math.Ceiling((double)total / userParams.PageSize)
            });




            return Ok(new ApiResponse<IEnumerable<UserDto>>(true, "Usuarios obtenidos correctamente.", userDtos));
        }


        [Authorize(Roles = "Admin")]
        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse<UserDto>>> GetUserById(string id)
        {
            var user = await _unitOfWork.UserRepository.GetUserByIdAsync(id);

            if (user == null)
            {
                return NotFound(new ApiResponse<string>(false, "Usuario no encontrado."));
            }

            var userDto = UserMapper.UserToUserDto(user);

            return Ok(new ApiResponse<UserDto>(true, "Usuario obtenido correctamente.", userDto));
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("{id}/status")]

        public async Task<ActionResult<ApiResponse<string>>> UpdateUserStatus(string id, [FromBody] UserStatusDto userStatusDto)
        {
            var user = await _unitOfWork.UserRepository.GetUserByIdAsync(id);

            if (user == null)
            {
                return NotFound(new ApiResponse<string>(false, "Usuario no encontrado."));
            }

            if(userStatusDto.Status == false){
                user.DeactivationReason = userStatusDto.Reason;
                user.Status = false;}
            else if(userStatusDto.Status == true){
                user.DeactivationReason = null;
                user.Status = true;}
            else{
                return BadRequest(new ApiResponse<string>(false, "El estado del usuario no es válido."));
            }
            await _unitOfWork.UserRepository.UpdateUserAsync(user);
            await _unitOfWork.SaveChangesAsync();

            return Ok(new ApiResponse<string>(true, "Estado del usuario actualizado correctamente."));
        }
    }


}