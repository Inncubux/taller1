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
            var query =  _unitOfWork.UserRepository.GetUsersQueryable();

            if(userParams.Status.HasValue)
            {
                query = query.Where(x => x.Status == userParams.Status.Value);
            }

            if(!string.IsNullOrWhiteSpace(userParams.SearchTerm))
            {
                var term = userParams.SearchTerm.ToLower();
                query = query.Where(x => x.FirstName.Contains(term, StringComparison.CurrentCultureIgnoreCase) ||
                                    x.LastName.Contains(term, StringComparison.CurrentCultureIgnoreCase) ||
                                    (x.Email != null && x.Email.Contains(term)));
            }

            if(userParams.RegistrationDateFrom.HasValue)
            {
                query = query.Where(x => x.RegistrationDate >= userParams.RegistrationDateFrom.Value);
            }
            if(userParams.RegistarionDateTo.HasValue)
            {
                query = query.Where(x => x.RegistrationDate <= userParams.RegistarionDateTo.Value);
            }

            var total = await query.CountAsync();

            var users = await query
                .OrderByDescending(x => x.RegistrationDate)
                .Skip((userParams.PageNumber - 1) * userParams.PageSize)
                .Take(userParams.PageSize)
                .ToListAsync();

            var userDtos = users.Select(x => UserMapper.UserToUserDto(x)).ToList();

            Response.AddPaginationHeader(new PaginationMetaData{
                CurrentPage = userParams.PageNumber,
                PageSize = userParams.PageSize,
                TotalCount = total,
                TotalPages = (int)Math.Ceiling((double)total / userParams.PageSize)
            });




            return Ok(new ApiResponse<IEnumerable<UserDto>>(true, "Usuarios obtenidos correctamente.", userDtos));
        }

    }
}