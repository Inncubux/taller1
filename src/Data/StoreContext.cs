using ECommerce.src.Models;
using Microsoft.EntityFrameworkCore;
using taller1.src.Models;
using taller1.src.Models.Relationship;

namespace ECommerce.src.Data
{
    public class StoreContext(DbContextOptions options) : DbContext(options)
    {
        public DbSet<Product> Products { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Address> Addresses { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Cart> Carts { get; set; }
        public DbSet<CartProduct> CartProducts { get; set; }
        public DbSet<OrderProduct> OrderProducts { get; set; }
        public DbSet<ProductImage> ProductImages { get; set; }
        
    }
}
