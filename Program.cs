using ECommerce.src.Data;
using ECommerce.src.Interfaces;
using ECommerce.src.Repositories;

using Microsoft.EntityFrameworkCore;

using Serilog;


Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateLogger();

try
{
    Log.Information("starting server.");
    var builder = WebApplication.CreateBuilder(args);

    builder.Services.AddControllers();
    builder.Services.AddDbContext<StoreContext>(options =>
        options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));
    builder.Host.UseSerilog((context, services, configuration) =>
    {
        configuration
            .ReadFrom.Configuration(context.Configuration)
            .Enrich.FromLogContext()
            .Enrich.WithThreadId()
            .Enrich.WithMachineName();
    });
    builder.Services.AddScoped<IProductRepository, ProductRepository>();
    builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

    var app = builder.Build();

    // Inicializar la base de datos
    DbInitializer.InitDb(app);

    // Registrar información importante sobre el servidor
    app.Lifetime.ApplicationStarted.Register(() =>
    {
        Log.Information($"Application started. Hosting environment: {app.Environment.EnvironmentName}");
        Log.Information($"Content root path: {app.Environment.ContentRootPath}");
        Log.Information($"Now listening on: {string.Join(", ", app.Urls)}");
    });

    // Mapear los controladores
    app.MapControllers();

    // Iniciar la aplicación
    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "server terminated unexpectedly");
}
finally
{
    Log.CloseAndFlush();
}