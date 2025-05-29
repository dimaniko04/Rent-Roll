using FluentValidation;

using Microsoft.Extensions.DependencyInjection;

using RentnRoll.Application.Services;
using RentnRoll.Application.Services.Validation;

namespace RentnRoll.Application;

public static class ApplicationDependencyInjection
{
    public static IServiceCollection AddApplication(
        this IServiceCollection services)
    {
        services.AddServices();

        services.AddValidatorsFromAssembly(
            typeof(IAssemblyMarker).Assembly,
            includeInternalTypes: true);

        return services;
    }

    private static void AddServices(
        this IServiceCollection services)
    {
        var assembly = typeof(IAssemblyMarker).Assembly;
        var serviceTypes = assembly.GetTypes()
            .Where(t =>
                t.IsClass &&
                !t.IsAbstract &&
                t.Name.EndsWith("Service"));

        foreach (var serviceType in serviceTypes)
        {
            var interfaceType = serviceType.GetInterfaces()
                .FirstOrDefault(i => i.Name == $"I{serviceType.Name}");

            if (interfaceType is not null)
            {
                services.AddScoped(interfaceType, serviceType);
            }
        }
    }
}