using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

using ECommerce.src.Dto;
using ECommerce.src.Models;

using Microsoft.AspNetCore.StaticAssets;

using taller1.src.Models;

namespace ECommerce.src.Mappers
{
    public class UserMapper
    {
        public static User RegisterToUser(RegisterDto registerDto) =>
        new()
        {
            UserName = registerDto.Email,
            Email = registerDto.Email,
            FirstName = registerDto.FirstName,
            LastName = registerDto.LastName,
            PhoneNumber = registerDto.Phone,
            Phone = registerDto.Phone,
            Password = registerDto.Password,
            RegistrationDate = DateTime.UtcNow,
            BirthDate = registerDto.BirthDate,
            Status = true,
            Address = new Address
            {
                Street = registerDto.Street ?? string.Empty,
                Number = registerDto.Number ?? string.Empty,
                Commune = registerDto.Commune ?? string.Empty,
                Region = registerDto.Region ?? string.Empty,
                PostalCode = registerDto.PostalCode ?? string.Empty,
            }
        };
        public static UserDto UserToUserDto(User user) =>
            new()
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email ?? string.Empty,
                Phone = user.PhoneNumber ?? string.Empty,
                Street = user.Address?.Street,
                Number = user.Address?.Number,
                Commune = user.Address?.Commune,
                Region = user.Address?.Region,
                PostalCode = user.Address?.PostalCode,
                RegistrationDate = user.RegistrationDate,
                LastLogin = user.LastLogin,
                Status = user.Status,
            };

        public static AuthUserDto UserToAuthUserDto(User user, string token) =>
            new()
            {
                FirtsName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email ?? string.Empty,
                Thelephone = user.PhoneNumber ?? string.Empty,
                Street = user.Address?.Street,
                Number = user.Address?.Number,
                Commune = user.Address?.Commune,
                Region = user.Address?.Region,
                PostalCode = user.Address?.PostalCode,
                RegistrationDate = user.RegistrationDate,
                LastLogin = user.LastLogin,
                Status = user.Status,
                Token = token
            };
    }
}