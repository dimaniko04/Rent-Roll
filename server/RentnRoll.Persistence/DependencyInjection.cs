using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RentnRoll.Persistence.Context;

namespace RentnRoll.Persistence;

public static class DependencyInjection
{
    public static IServiceCollection AddPersistence(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddDbContext<RentnRollDbContext>(options =>
            options.UseSqlServer(configuration
                .GetConnectionString("DefaultConnection")));

        return services;
    }
}