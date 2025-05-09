using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using ECommerce.src.Models;

namespace ECommerce.src.Interfaces
{
    public interface IUserRepository
    {
        IQueryable<User> GetUsersQueryable();
        Task<User> GetUserByIdAsync(string id);
        Task<User> GetUserByEmailAsync(string email);
        Task<User?> UpdateUserAsync(User user);
        Task<User?> GetByEmailAsync(string id);
        Task<bool> CheckPasswordAsync(User user, string password);
    }
}