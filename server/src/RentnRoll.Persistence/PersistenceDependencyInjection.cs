using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using RentnRoll.Application.Common.Interfaces.Repositories;
using RentnRoll.Application.Common.Interfaces.UnitOfWork;
using RentnRoll.Persistence.Context;
using RentnRoll.Persistence.Repositories;
using RentnRoll.Persistence.UnitOfWork;

namespace RentnRoll.Persistence;

public static class PersistenceDependencyInjection
{
    public static IServiceCollection AddPersistence(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddDbContext<RentnRollDbContext>(options =>
            options.UseSqlServer(configuration
                .GetConnectionString("DefaultConnection")));

        services.AddScoped<ITestEntityRepository, TestEntityRepository>();

        services.AddScoped<IUnitOfWork, RentnRollUnitOfWork>();

        return services;
    }
}