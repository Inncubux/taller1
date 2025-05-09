using System.Globalization;
using System.Security.Claims;

using ECommerce.src.Data;
using ECommerce.src.Interfaces;
using ECommerce.src.Middleware;
using ECommerce.src.Models;
using ECommerce.src.Repositories;
using ECommerce.src.Services;

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

    // Configurar el formato de fecha global
    var cultureInfo = new CultureInfo("es-ES");
    cultureInfo.DateTimeFormat.ShortDatePattern = "dd-MM-yyyy";
    cultureInfo.DateTimeFormat.DateSeparator = "-";

    // Establecer la configuración global
    CultureInfo.DefaultThreadCurrentCulture = cultureInfo;
    CultureInfo.DefaultThreadCurrentUICulture = cultureInfo;

    builder.Services.AddControllers();
    // Configure middleware for exception handling
    builder.Services.AddTransient<ExceptionMiddleware>();
    // Configure database context
    builder.Services.AddDbContext<StoreContext>(options =>
        options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));
    
    // Configure serilog for logging
    builder.Host.UseSerilog((context, services, configuration) =>
    {
        configuration
            .ReadFrom.Configuration(context.Configuration)
            .Enrich.FromLogContext()
            .Enrich.WithThreadId()
            .Enrich.WithMachineName();
    });

    // Configure scoped services
    builder.Services.AddScoped<IProductRepository, ProductRepository>();
    builder.Services.AddScoped<IUserRepository, UserRepository>();
    builder.Services.AddScoped<ITokenService, TokenService>();
    builder.Services.AddScoped<UnitOfWork>();
    builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

    // Configure Identity
    builder.Services.AddIdentity<User, IdentityRole>(opt =>
    {
        opt.User.RequireUniqueEmail = true;
        opt.Password.RequireNonAlphanumeric = false;
        opt.Password.RequiredLength = 6;
        opt.SignIn.RequireConfirmedEmail = false;
    })
        .AddEntityFrameworkStores<StoreContext>();

    // Configure Authentication
    // Add JWT authentication
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
            IssuerSigningKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(builder.Configuration["JWT:SignInKey"]!)),
            RoleClaimType = ClaimTypes.Role
        };
    });

    var allowedOrigins = builder.Configuration.GetSection("AllowedOrigins").Get<string[]>() ?? Array.Empty<string>();
    // Configure CORS
    builder.Services.AddCors(opt =>
    {
        opt.AddPolicy("CorsPolicy", policy =>
        {
            policy.AllowAnyHeader()
                .AllowAnyMethod()
                .WithOrigins(allowedOrigins);
        });
    });

    // Configure Distributed Memory Cache for active sessions
    builder.Services.AddDistributedMemoryCache();

    // Configure session state
    builder.Services.AddSession(opt =>
    {
        opt.IdleTimeout = TimeSpan.FromMinutes(30);
        opt.Cookie.HttpOnly = true;
        opt.Cookie.IsEssential = true; // Make the session cookie essential
    });

    var app = builder.Build();
    app.UseMiddleware<ExceptionMiddleware>();
    app.UseCors("CorsPolicy");
    app.UseSession();
    app.UseAuthentication();
    app.UseAuthorization();


    // Inicializar la base de datos
    await Seeder.InitDb(app);

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