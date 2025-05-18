using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using ECommerce.src.Models;

namespace ECommerce.src.Interfaces
{
    /// <summary>
    /// Interface for JWT token generation service.
    /// Provides a method to generate a JWT token for a given user and role.
    /// </summary>
    public interface ITokenService
    {
        /// <summary>
        /// Generates a JWT token for the specified user and role.
        /// </summary>
        /// <param name="user">The user for whom the token is generated.</param>
        /// <param name="role">The role of the user.</param>
        /// <returns>A JWT token as a string.</returns>
        string GenerateToken(User user, string role);
    }
}