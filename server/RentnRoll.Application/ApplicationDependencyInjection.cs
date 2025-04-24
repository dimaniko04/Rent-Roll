using FluentValidation;

using Microsoft.Extensions.DependencyInjection;

using RentnRoll.Application.Common.Interfaces.Services;
using RentnRoll.Application.Services;

namespace RentnRoll.Application;

public static class ApplicationDependencyInjection
{
    public static IServiceCollection AddApplication(
        this IServiceCollection services)
    {
        services.AddScoped<ITestEntityService, TestEntityService>();

        services.AddValidatorsFromAssembly(
            typeof(IAssemblyMarker).Assembly,
            includeInternalTypes: true);

        return services;
    }
}