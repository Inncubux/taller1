using ECommerce.src.Models;

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

using taller1.src.Models;
using taller1.src.Models.Relationship;

namespace ECommerce.src.Data
{
    /// <summary>
    /// Database context for the e-commerce application. It inherits from DbContext to provide access to the database.
    /// </summary>
    public class StoreContext(DbContextOptions<StoreContext> options) : IdentityDbContext<User>(options)
    {
        /// <summary>
        /// Entity set for the products in the database. It represents the Products table in the database.
        /// </summary>
        public DbSet<Product> Products { get; set; }

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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>()
                .HasOne(u => u.Address)
                .WithOne(a => a.User)
                .HasForeignKey<Address>(a => a.UserId);

            List<IdentityRole> roles =
            [
                new IdentityRole {Id = "1", Name = "Admin", NormalizedName = "ADMIN" },
                new IdentityRole {Id = "2", Name = "User", NormalizedName = "USER" }
            ];

            modelBuilder.Entity<IdentityRole>().HasData(roles);

            modelBuilder.Entity<Cart>()
                .HasOne(c => c.User)
                .WithOne(u => u.Cart)
                .HasForeignKey<Cart>(c => c.UserId);
        }
    }
}