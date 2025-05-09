using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using ECommerce.src.Interfaces;

namespace ECommerce.src.Data
{
    public class UnitOfWork(StoreContext context, IUserRepository userRepository, IProductRepository productRepository)
    {
        private readonly StoreContext _context = context;

        public IUserRepository UserRepository { get; } = userRepository;
        public IProductRepository ProductRepository { get; } = productRepository;

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}