using System.Threading.Tasks;

using Bogus;

using ECommerce.src.Dto;
using ECommerce.src.Mappers;
using ECommerce.src.Models;

using Microsoft.AspNetCore.Identity;
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
        public static async Task InitDb(WebApplication app)
        {
            using var scope = app.Services.CreateScope();
            using var userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>()
                ?? throw new InvalidOperationException("Could not get UserManager<User>");

            var context = scope.ServiceProvider.GetRequiredService<StoreContext>()
                ?? throw new InvalidOperationException("Could not get StoreContext");

            SeedData(context);
            await CreateUsers(userManager, GenerateUserDtos(10));

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
            var faker = new Faker("es");

            if (!context.Products.Any())
            {

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

                if (!context.ProductImages.Any())
                {
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
                    context.SaveChanges(); // Save product images
                }
            }
        }

        private static List<RegisterDto> GenerateUserDtos(int count = 10)
        {
            var faker = new Faker("es");
            var users = new Faker<RegisterDto>()
                .RuleFor(u => u.FirstName, f => f.Name.FirstName())
                .RuleFor(u => u.LastName, f => f.Name.LastName())
                .RuleFor(u => u.Email, f => f.Internet.Email())
                .RuleFor(u => u.Password, f => f.Internet.Password(8, false)
                         + f.Random.Char('A', 'Z')
                         + f.Random.Char('a', 'z')
                         + f.Random.Char('0', '9')
                         + f.PickRandom('!', '@', '#', '$', '%', '^', '&', '*'))
                .RuleFor(u => u.Phone, f => f.Phone.PhoneNumber())
                .RuleFor(u => u.BirthDate, f => f.Date.Past(30, DateTime.Now.AddYears(-18)))
                .RuleFor(u => u.Street, f => f.Address.StreetName())
                .RuleFor(u => u.Number, f => f.Address.BuildingNumber())
                .RuleFor(u => u.Commune, f => f.Address.City())
                .RuleFor(u => u.Region, f => f.Address.State())
                .RuleFor(u => u.PostalCode, f => f.Address.ZipCode())
                .Generate(count);

            return users;
        }

        public static async Task CreateUsers(UserManager<User> userManager, List<RegisterDto> users)
        {
            if (!userManager.Users.Any())
            {
                var admin = new User
                {
                    UserName = "ignacio.mancilla@gmail.com",
                    Email = "ignacio.mancilla@gmail.com",
                    FirstName = "Ignacio",
                    LastName = "Mancilla",
                    Phone = "999999999",
                    Password = "Pa$$word2025",
                    RegistrationDate = DateTime.UtcNow,
                    Status = true,
                    BirthDate = new DateTime(1990, 1, 1),
                    Address = new Address
                    {
                        Street = "Central",
                        Number = "1000",
                        Commune = "Santiago",
                        Region = "RM",
                        PostalCode = "0000000"
                    }
                };

                var existingAdmin = await userManager.FindByEmailAsync(admin.Email);
                if (existingAdmin == null)
                {
                    var result = await userManager.CreateAsync(admin, admin.Password);
                    if (result.Succeeded)
                    {
                        await userManager.AddToRoleAsync(admin, "Admin");

                    }
                    else
                    {
                        throw new Exception($"Error creating admin user: {string.Join(", ", result.Errors.Select(e => e.Description))}");
                    }
                }

                foreach (var userDto in users)
                {
                    var user = UserMapper.RegisterToUser(userDto);
                    user.UserName = userDto.Email;
                    user.Email = userDto.Email;
                    var result = await userManager.CreateAsync(user, userDto.Password);
                    if (result.Succeeded)
                    {
                        await userManager.AddToRoleAsync(user, "User");

                    }
                    else
                    {

                        throw new Exception($"Error creating user: {string.Join(", ", result.Errors.Select(e => e.Description))}");
                    }

                }
            }
        }
    }
}