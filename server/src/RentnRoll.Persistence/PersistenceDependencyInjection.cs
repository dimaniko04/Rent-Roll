using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using RentnRoll.Application.Common.Interfaces.UnitOfWork;
using RentnRoll.Persistence.Context;
using RentnRoll.Persistence.Interceptors;
using RentnRoll.Persistence.UnitOfWork;

namespace RentnRoll.Persistence;

public static class PersistenceDependencyInjection
{
    public static IServiceCollection AddPersistence(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var connectionString = configuration
            .GetConnectionString("DefaultConnection");

        services.AddSingleton<SoftDeleteInterceptor>();

        services.AddDbContext<RentnRollDbContext>((sp, options) =>
            options.UseSqlServer(connectionString)
                .AddInterceptors(
                    sp.GetRequiredService<SoftDeleteInterceptor>()));

        services.AddScoped<IUnitOfWork, RentnRollUnitOfWork>();

        return services;
    }
}