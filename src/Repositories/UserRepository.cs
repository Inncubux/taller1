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

        public async Task<User?> GetUserByEmailAsync(string email)
        {
            return await _userManager.Users.Include(u => u.Address).FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task<User?> GetUserByIdAsync(string id)
        {
            return await _userManager.Users.Include(u => u.Address).FirstOrDefaultAsync(u => u.Id == id);
        }

        public IQueryable<User> GetUsersQueryable()
        {
            return _userManager.Users.Include(u => u.Address).AsQueryable();
        }

        public async Task UpdateUserAsync(User user)
        {
            await _userManager.UpdateAsync(user);
        }
    }
}