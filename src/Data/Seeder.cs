using Bogus;

using ECommerce.src.Models;

using Microsoft.EntityFrameworkCore;

using taller1.src.Models;
using taller1.src.Models.Relationship;

namespace ECommerce.src.Data
{
    /// <summary>
    /// Class responsible for initializing the database with sample data.
    /// </summary>
    public class Seeder
    {
        /// <summary>
        /// Method to initialize the database with sample data.
        /// This method should be called at the start of the application to ensure that the database is seeded with data.
        /// </summary>
        /// <param name="app"></param>
        public static void InitDb(WebApplication app)
        {
            using var scope = app.Services.CreateScope();

            var context = scope.ServiceProvider.GetRequiredService<StoreContext>()
                ?? throw new InvalidOperationException("Could not get StoreContext");

            SeedData(context);
        }
        /// <summary>
        /// Method to seed the database with sample data.
        /// This method generates fake data for users and products using the Bogus library.
        /// </summary>
        /// <param name="context">Variable for StoreContext</param>
        private static void SeedData(StoreContext context)
        {
            // Verify if the database exists and create it if it doesn't.
            context.Database.Migrate();

            // Verify if the database is empty before seeding data.
            // This prevents duplicate data from being added to the database.
            if (context.Users.Any() || context.Products.Any()) return;

            var faker = new Faker("es");

            // Generate 10 fake users using the Bogus library.
            var users = new Faker<User>()
                .RuleFor(u => u.FullName, f => f.Name.FullName())
                .RuleFor(u => u.Email, f => f.Internet.Email())
                .RuleFor(u => u.Phone, f => f.Phone.PhoneNumber())
                .RuleFor(u => u.BirthDate, f => f.Date.Past(30, DateTime.Now.AddYears(-18)))
                .RuleFor(u => u.Password, f => f.Internet.Password())
                .RuleFor(u => u.Role, f => f.PickRandom(0, 1)) // Assigns a Role value
                .Generate(10);

            context.Users.AddRange(users);
            context.SaveChanges(); // Guarda los usuarios antes de continuar

            // Generate 20 fake products using the Bogus library.
            var products = new Faker<Product>()
            .RuleFor(p => p.Title, f => f.Commerce.ProductName())
            .RuleFor(p => p.Category, f => f.Commerce.Department())
            .RuleFor(p => p.Description, f => f.Commerce.ProductDescription())
            .RuleFor(p => p.Price, f => f.Random.Decimal(5000, 50000))
            .RuleFor(p => p.Brand, f => f.Company.CompanyName())
            .RuleFor(p => p.Stock, f => f.Random.Int(10, 200))
            .RuleFor(p => p.Condition, f => f.PickRandom("Nuevo", "Usado", "Reacondicionado")) // Sets the condition
            .Generate(20);

            context.Products.AddRange(products);
            context.SaveChanges(); // Save products before generating cart products

            // Generate product images using the Bogus library.
            var productImages = new Faker<ProductImage>()
                .RuleFor(pi => pi.ImageUrl, f => f.Image.PicsumUrl()) // Generates a random image URL
                .RuleFor(pi => pi.ProductId, f => f.Random.Int(1, 20)); // Associates the image with an existing product

            // Generate images for each product
            var images = new List<ProductImage>();
            foreach (var product in products)
            {
                var imagesForProduct = productImages.Generate(faker.Random.Int(1, 5)); // Generates between 1 and 5 images per product
                foreach (var image in imagesForProduct)
                {
                    image.ProductId = product.Id; // Associates the image with the current product
                }
                images.AddRange(imagesForProduct);
            }

            context.ProductImages.AddRange(images);

            // Generate 10 fake addresses using the Bogus library.
            var addresses = new Faker<Address>()
                .RuleFor(a => a.Street, f => f.Address.StreetName())
                .RuleFor(a => a.Number, f => f.Address.BuildingNumber())
                .RuleFor(a => a.Commune, f => f.Address.City())
                .RuleFor(a => a.Region, f => f.Address.State())
                .RuleFor(a => a.PostalCode, f => f.Address.ZipCode())
                .RuleFor(a => a.IsDefault, f => f.Random.Bool())
                .RuleFor(a => a.UserId, f => f.PickRandom(users).Id) // Asocia con un usuario existente
                .Generate(10); // Generates 10 addresses

            context.Addresses.AddRange(addresses);

            // Generate 10 fake carts using the Bogus library.
            var carts = new Faker<Cart>()
                .RuleFor(c => c.UserId, f => f.Random.Int(1, 10)) // Associates the cart with an existing user
                .Generate(10); // Generates 10 carts

            context.Carts.AddRange(carts);
            context.SaveChanges(); // Save carts before generating cart products

            var orders = new Faker<Order>()
                .RuleFor(o => o.UserId, f => f.Random.Int(1, 10)) // Associates the order with an existing user
                .RuleFor(o => o.AddressId, f => f.Random.Int(1, 10)) // Associates the order with an existing address
                .RuleFor(o => o.PurchaseDate, f => f.Date.Past(1)) // Purchase date within the last year
                .RuleFor(o => o.Total, f => f.Random.Decimal(1000, 50000)) // Total amount of the order
                .RuleFor(o => o.OrderStatus, f => f.PickRandom("pending", "shipped", "delivered")) // Order status
                .Generate(10); // Generates 10 orders

            context.Orders.AddRange(orders);

            context.SaveChanges();
        }

    }

}