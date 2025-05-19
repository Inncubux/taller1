using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using ECommerce.src.Interfaces;
using ECommerce.src.Models;

using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.src.Repositories
{
    /// <summary>
    /// Repository implementation for user-related data operations.
    /// Provides methods for querying, retrieving, updating users, and changing user passwords using UserManager.
    /// </summary>
    public class UserRepository(UserManager<User> userManager) : IUserRepository
    {
        private readonly UserManager<User> _userManager = userManager;

        /// <summary>
        /// Changes the password of the specified user.
        /// Hashes the new password and updates the user entity.
        /// </summary>
        /// <param name="user">The user whose password will be changed.</param>
        /// <param name="newPassword">The new password to set.</param>
        /// <returns>An IdentityResult indicating the outcome of the operation.</returns>
        public Task<IdentityResult> ChangePasswordAsync(User user, string newPassword)
        {
            var hasher = new PasswordHasher<User>();
            user.PasswordHash = hasher.HashPassword(user, newPassword);
            return _userManager.UpdateAsync(user);
        }

        /// <summary>
        /// Retrieves a user by their email address asynchronously, including their address.
        /// </summary>
        /// <param name="email">The user's email address.</param>
        /// <returns>The user entity if found; otherwise, null.</returns>
        public async Task<User?> GetUserByEmailAsync(string email)
        {
            return await _userManager.Users.Include(u => u.Address).FirstOrDefaultAsync(u => u.Email == email);
        }

        /// <summary>
        /// Retrieves a user by their unique identifier asynchronously, including their address.
        /// </summary>
        /// <param name="id">The user's unique identifier.</param>
        /// <returns>The user entity if found; otherwise, null.</returns>
        public async Task<User?> GetUserByIdAsync(string id)
        {
            return await _userManager.Users.Include(u => u.Address).FirstOrDefaultAsync(u => u.Id == id);
        }

        /// <summary>
        /// Returns a queryable collection of users, including their addresses, for further filtering or pagination.
        /// </summary>
        /// <returns>IQueryable of User entities.</returns>
        public IQueryable<User> GetUsersQueryable()
        {
            return _userManager.Users.Include(u => u.Address).AsQueryable();
        }

        /// <summary>
        /// Updates the specified user entity asynchronously.
        /// </summary>
        /// <param name="user">The user entity to update.</param>
        public async Task UpdateUserAsync(User user)
        {
            await _userManager.UpdateAsync(user);
        }
    }
}