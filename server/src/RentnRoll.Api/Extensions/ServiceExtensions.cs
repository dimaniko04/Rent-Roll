using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Models;

using RentnRoll.Api.Middlewares;

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
            c.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "RentnRoll API",
                Version = "v1"
            });

            c.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                    },
                    []
                }
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
}