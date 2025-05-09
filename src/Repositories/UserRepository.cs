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
    public class UserRepository(UserManager<User> userManager) : IUserRepository
    {
        private readonly UserManager<User> _userManager = userManager;

        public async Task<bool> CheckPasswordAsync(User user, string password)
        {
            var hasher = new PasswordHasher<User>();
            var result = hasher.VerifyHashedPassword(user, user.PasswordHash!, password);
            return result == PasswordVerificationResult.Success;
        }

        public async Task<User?> GetByEmailAsync(string id)
        {
            throw new NotImplementedException();
        }

        public async Task<User> GetUserByEmailAsync(string email)
        {
            throw new NotImplementedException();
        }

        public async Task<User> GetUserByIdAsync(string id)
        {
            throw new NotImplementedException();
        }

        public IQueryable<User> GetUsersQueryable()
        {
            return _userManager.Users.Include(u => u.Address).AsQueryable();
        }

        public async Task<User?> UpdateUserAsync(User user)
        {
            throw new NotImplementedException();
        }
    }
}