using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using ECommerce.src.Models;

using Microsoft.AspNetCore.Identity;

namespace ECommerce.src.Interfaces
{
    /// <summary>
    /// Interface for user repository operations.
    /// Provides methods for querying, retrieving, updating users, and changing user passwords.
    /// </summary>
    public interface IUserRepository
    {
        /// <summary>
        /// Returns a queryable collection of users for further filtering or pagination.
        /// </summary>
        /// <returns>IQueryable of User entities.</returns>
        IQueryable<User> GetUsersQueryable();

        /// <summary>
        /// Retrieves a user by their unique identifier asynchronously.
        /// </summary>
        /// <param name="id">The user's unique identifier.</param>
        /// <returns>The user entity if found; otherwise, null.</returns>
        Task<User?> GetUserByIdAsync(string id);

        /// <summary>
        /// Retrieves a user by their email address asynchronously.
        /// </summary>
        /// <param name="email">The user's email address.</param>
        /// <returns>The user entity if found; otherwise, null.</returns>
        Task<User?> GetUserByEmailAsync(string email);

        /// <summary>
        /// Updates the specified user entity asynchronously.
        /// </summary>
        /// <param name="user">The user entity to update.</param>
        Task UpdateUserAsync(User user);

        /// <summary>
        /// Changes the password of the specified user asynchronously.
        /// </summary>
        /// <param name="user">The user whose password will be changed.</param>
        /// <param name="newPassword">The new password to set.</param>
        /// <returns>An IdentityResult indicating the outcome of the operation.</returns>
        Task<IdentityResult> ChangePasswordAsync(User user, string newPassword);
    }
}