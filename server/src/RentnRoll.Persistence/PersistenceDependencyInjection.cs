using System.Text;

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

using RentnRoll.Application.Common.Interfaces.Identity;
using RentnRoll.Application.Common.Interfaces.UnitOfWork;
using RentnRoll.Application.Common.Policies;
using RentnRoll.Persistence.Context;
using RentnRoll.Persistence.Identity;
using RentnRoll.Persistence.Identity.Services;
using RentnRoll.Persistence.Interceptors;
using RentnRoll.Persistence.Requirements;
using RentnRoll.Persistence.Requirements.Businesses;
using RentnRoll.Persistence.Requirements.Games;
using RentnRoll.Persistence.Seeding;
using RentnRoll.Persistence.Settings;
using RentnRoll.Persistence.UnitOfWork;

namespace RentnRoll.Persistence;

public static class PersistenceDependencyInjection
{
    public static IServiceCollection AddPersistence(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var jwtSettings = new JwtSettings();
        configuration
            .GetSection(JwtSettings.SectionName)
            .Bind(jwtSettings);
        services.Configure<AdminSettings>(
            configuration.GetSection(AdminSettings.SectionName));

        services
            .AddIdentity()
            .AddJwtAuthentication(jwtSettings)
            .AddAuthorization()
            .AddDbContext(configuration)
            .AddRepositories();

        services.AddScoped<Seeder>();

        services.AddSingleton(Options.Create(jwtSettings));

        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<ITokenService, TokenService>();

        services.AddScoped<IUnitOfWork, RentnRollUnitOfWork>();

        return services;
    }

    private static IServiceCollection AddDbContext(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var connectionString = configuration
            .GetConnectionString("DefaultConnection");

        services.AddSingleton<SoftDeleteInterceptor>();
        services.AddSingleton<UpdateAuditableInterceptor>();

        services.AddDbContext<RentnRollDbContext>((sp, options) =>
            options.UseSqlServer(connectionString)
                .AddInterceptors(
                    sp.GetRequiredService<SoftDeleteInterceptor>(),
                    sp.GetRequiredService<UpdateAuditableInterceptor>()));

        return services;
    }

    private static void AddRepositories(
        this IServiceCollection services)
    {
        var assembly = typeof(IAssemblyMarker).Assembly;
        var repositoryTypes = assembly.GetTypes()
            .Where(t =>
                t.IsClass &&
                !t.IsAbstract &&
                t.Name.EndsWith("Repository"));

        foreach (var repositoryType in repositoryTypes)
        {
            var interfaceType = repositoryType.GetInterfaces()
                .FirstOrDefault(i => i.Name == $"I{repositoryType.Name}");

            if (interfaceType is not null)
            {
                services.AddScoped(interfaceType, repositoryType);
            }
        }
    }

    private static IServiceCollection AddJwtAuthentication(
        this IServiceCollection services,
        JwtSettings jwtSettings)
    {
        services
            .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                var key = Encoding.UTF8.GetBytes(jwtSettings.Key);

                options.RequireHttpsMetadata = true;
                options.SaveToken = true;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = jwtSettings.Issuer,
                    ValidAudience = jwtSettings.Audience,
                    IssuerSigningKey = new SymmetricSecurityKey(key)
                };
            });

        return services;
    }

    private static IServiceCollection AddAuthorization(
        this IServiceCollection services)
    {
        services.AddAuthorization(options =>
        {
            options.AddPolicy(Policy.CreatorOrAdmin, policy =>
                policy.Requirements.Add(
                    new IsGameCreatorOrAdminRequirement()));
            options.AddPolicy(Policy.OwnerOnly, policy =>
                policy.Requirements.Add(
                    new IsBusinessOwnerRequirement()));
            options.AddPolicy(Policy.OwnerOrAdmin, policy =>
                policy.Requirements.Add(
                    new IsBusinessOwnerOrAdminRequirement()));
        });

        services.AddScoped<IAuthorizationHandler, IsGameCreatorOrAdminHandler>();
        services.AddScoped<IAuthorizationHandler, IsBusinessOwnerHandler>();
        services.AddScoped<IAuthorizationHandler, IsBusinessOwnerOrAdminHandler>();


        return services;
    }

    private static IServiceCollection AddIdentity(
        this IServiceCollection services)
    {
        services.AddIdentityCore<User>()
            .AddRoles<IdentityRole>()
            .AddEntityFrameworkStores<RentnRollDbContext>();

        services.Configure<IdentityOptions>(options =>
        {
            options.User.RequireUniqueEmail = true;

            options.SignIn.RequireConfirmedEmail = false;
            options.SignIn.RequireConfirmedPhoneNumber = false;

            options.Password.RequireDigit = false;
            options.Password.RequiredLength = 8;
            options.Password.RequireLowercase = false;
            options.Password.RequireUppercase = false;
            options.Password.RequireNonAlphanumeric = false;

            options.Lockout.AllowedForNewUsers = true;
            options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
            options.Lockout.MaxFailedAccessAttempts = 5;
        });

        return services;
    }
}