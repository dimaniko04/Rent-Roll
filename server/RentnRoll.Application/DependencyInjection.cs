using Microsoft.Extensions.DependencyInjection;
using RentnRoll.Application.Interfaces.Services;
using RentnRoll.Application.Services;

namespace RentnRoll.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(
        this IServiceCollection services)
    {
        services.AddSingleton<ITestEntityService, TestEntityService>();

        return services;
    }
}