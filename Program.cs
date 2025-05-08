using System.Security.Claims;

using ECommerce.src.Data;
using ECommerce.src.Interfaces;
using ECommerce.src.Models;
using ECommerce.src.Repositories;

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

using Serilog;

/// <summary>
/// Main entry point for the application. It sets up the server, configures services, and starts the application.
/// </summary>
/// <returns>Sets up the server</returns>
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
    builder.Services.AddIdentity<User, IdentityRole>(opt =>
    {
        opt.User.RequireUniqueEmail = true;
        opt.Password.RequireNonAlphanumeric = false;
        opt.Password.RequiredLength = 6;
        opt.SignIn.RequireConfirmedEmail = false;
    })
        .AddEntityFrameworkStores<StoreContext>();

    builder.Services.AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
    }).AddJwtBearer(opt =>
    {
        opt.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = builder.Configuration["JWT:Issuer"],
            ValidateAudience = true,
            ValidAudience = builder.Configuration["JWT:Audience"],
            ValidateIssuerSigningKey = true,
            ValidateLifetime = true,
            IssuerSigningKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(builder.Configuration["JWT:SigningKey"]!)),
            RoleClaimType = ClaimTypes.Role
        };
    });

    var app = builder.Build();
    app.UseAuthentication();
    app.UseAuthorization();
    

    // Inicializar la base de datos
    Seeder.InitDb(app);

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