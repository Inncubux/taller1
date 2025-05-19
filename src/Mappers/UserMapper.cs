using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

using ECommerce.src.Dto.User;
using ECommerce.src.Models;

using Microsoft.AspNetCore.StaticAssets;

using taller1.src.Models;

namespace ECommerce.src.Mappers
{
    /// <summary>
    /// Provides mapping methods between User entities and their corresponding DTOs.
    /// </summary>
    public class UserMapper
    {
        /// <summary>
        /// Maps a RegisterDto to a User entity.
        /// </summary>
        /// <param name="registerDto">The registration DTO containing user information.</param>
        /// <returns>A User entity populated with the registration data.</returns>
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

        /// <summary>
        /// Maps a User entity to a UserDto.
        /// </summary>
        /// <param name="user">The User entity to map.</param>
        /// <returns>A UserDto containing user profile information.</returns>
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

        /// <summary>
        /// Maps a User entity to an AuthUserDto, including a JWT token.
        /// </summary>
        /// <param name="user">The User entity to map.</param>
        /// <param name="token">The JWT token to include.</param>
        /// <returns>An AuthUserDto containing user profile information and the JWT token.</returns>
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