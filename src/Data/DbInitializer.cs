using Bogus;

using ECommerce.src.Models;

using Microsoft.EntityFrameworkCore;

namespace ECommerce.src.Data
{
    public class DbInitializer
    {
        public static void InitDb(WebApplication app)
        {
            using var scope = app.Services.CreateScope();

            var context = scope.ServiceProvider.GetRequiredService<StoreContext>()
                ?? throw new InvalidOperationException("Could not get StoreContext");

            SeedData(context);
        }

        private static void SeedData(StoreContext context)
        {
            // Ejecuta las migraciones pendientes al iniciar la app
            context.Database.Migrate();

            // Verifica si ya existen usuarios o productos para evitar duplicados
            if (context.Users.Any() || context.Products.Any()) return;

            var faker = new Faker("es");

            // Genera 10 usuarios falsos
            var users = new Faker<User>()
                .RuleFor(u => u.FullName, f => f.Name.FullName())
                .RuleFor(u => u.Email, f => f.Internet.Email())
                .RuleFor(u => u.Phone, f => f.Phone.PhoneNumber())
                .RuleFor(u => u.BirthDate, f => f.Date.Past(30, DateTime.Now.AddYears(-18)))
                .RuleFor(u => u.Password, f => f.Internet.Password())
                .RuleFor(u => u.Role, f => f.PickRandom(0, 1)) // Asigna un valor de Role
                .Generate(10);

            context.Users.AddRange(users);

            // Genera 20 productos falsos
            var products = new Faker<Product>()
            .RuleFor(p => p.Title, f => f.Commerce.ProductName())
            .RuleFor(p => p.Category, f => f.Commerce.Department())
            .RuleFor(p => p.Description, f => f.Commerce.ProductDescription())
            .RuleFor(p => p.Price, f => f.Random.Decimal(5000, 50000))
            .RuleFor(p => p.Brand, f => f.Company.CompanyName())
            .RuleFor(p => p.Stock, f => f.Random.Int(10, 200))
            .RuleFor(p => p.Condition, f => f.PickRandom("Nuevo", "Usado", "Reacondicionado")) // Agregar este valor
            .Generate(20);


            context.Products.AddRange(products);

            context.SaveChanges();
        }

    }

}