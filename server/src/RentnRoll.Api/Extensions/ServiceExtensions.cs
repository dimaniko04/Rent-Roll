using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

using RentnRoll.Api.Middlewares;
using RentnRoll.Persistence.Context;
using RentnRoll.Persistence.Identity;

namespace RentnRoll.Api.Extensions;

public static class ServiceExtensions
{
    public static IServiceCollection AddPresentation(
        this IServiceCollection services,
        string corsPolicyName)
    {
        services.AddCorsPolicy(corsPolicyName);
        services.AddSwagger();
        services.AddControllers();
        services.AddIdentity();
        services.AddExceptionHandler<GlobalExceptionHandler>();
        services.AddProblemDetails();

        services.Configure<ApiBehaviorOptions>(options =>
        {
            options.SuppressModelStateInvalidFilter = true;
        });

        return services;
    }

    private static IServiceCollection AddSwagger(
        this IServiceCollection services)
    {
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
            {
                Title = "RentnRoll API",
                Version = "v1"
            });
        });

        return services;
    }

    private static IServiceCollection AddCorsPolicy(
        this IServiceCollection services,
        string policyName)
    {
        services.AddCors(options =>
        {
            options.AddPolicy(name: policyName,
                corsPolicyBuilder =>
                {
                    corsPolicyBuilder.AllowAnyOrigin();
                    corsPolicyBuilder.AllowAnyMethod();
                    corsPolicyBuilder.AllowAnyHeader();
                });
        });

        return services;
    }

    private static IServiceCollection AddIdentity(
        this IServiceCollection services)
    {
        services.AddAuthorization();
        services.AddAuthentication()
            .AddBearerToken(IdentityConstants.BearerScheme);

        services.AddIdentityCore<User>()
            .AddRoles<IdentityRole>()
            .AddEntityFrameworkStores<RentnRollDbContext>()
            .AddApiEndpoints();

        return services;
    }
}