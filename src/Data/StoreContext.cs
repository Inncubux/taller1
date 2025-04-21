using ECommerce.src.Models;

using Microsoft.EntityFrameworkCore;

using taller1.src.Models;
using taller1.src.Models.Relationship;

namespace ECommerce.src.Data
{
    /// <summary>
    /// Database context for the e-commerce application. It inherits from DbContext to provide access to the database.
    /// </summary>
    public class StoreContext(DbContextOptions options) : DbContext(options)
    {
        /// <summary>
        /// Entity set for the products in the database. It represents the Products table in the database.
        /// </summary>
        public DbSet<Product> Products { get; set; }
        /// <summary>
        /// Entity set for the users in the database. It represents the Users table in the database.
        /// </summary>
        public DbSet<User> Users { get; set; }
        /// <summary>
        /// Entity set for the addresses in the database. It represents the Addresses table in the database.
        /// </summary>
        public DbSet<Address> Addresses { get; set; }
        /// <summary>
        /// Entity set for the orders in the database. It represents the Categories table in the database.
        /// </summary>
        public DbSet<Order> Orders { get; set; }
        /// <summary>
        /// Entity set for the carts in the database. It represents the Orders table in the database.
        /// </summary>
        public DbSet<Cart> Carts { get; set; }
        /// <summary>
        /// Entity set for the cart products in the database. It represents the CartProducts table in the database.
        /// </summary>
        public DbSet<CartProduct> CartProducts { get; set; }
        /// <summary>
        /// Entity set for the order products in the database. It represents the OrderProducts table in the database.
        /// </summary>
        public DbSet<OrderProduct> OrderProducts { get; set; }
        /// <summary>
        /// Entity set for the product images in the database. It represents the ProductImages table in the database.
        /// </summary>
        public DbSet<ProductImage> ProductImages { get; set; }

    }
}