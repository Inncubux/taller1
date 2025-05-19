using System;
using System.Threading.Tasks;
using ECommerce.src.Interfaces;

namespace ECommerce.src.Data
{
    public class UnitOfWork : IDisposable
    {
        private readonly StoreContext _context;

        public IUserRepository UserRepository { get; }
        public IProductRepository ProductRepository { get; }

        public UnitOfWork(StoreContext context, IUserRepository userRepository, IProductRepository productRepository)
        {
            _context = context;
            UserRepository = userRepository;
            ProductRepository = productRepository;
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
