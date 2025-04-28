using FluentValidation;

using Microsoft.Extensions.DependencyInjection;

using RentnRoll.Application.Services.Validation;

namespace RentnRoll.Application;

public static class ApplicationDependencyInjection
{
    public static IServiceCollection AddApplication(
        this IServiceCollection services)
    {
        services.AddScoped<IValidationService, ValidationService>();

        services.AddValidatorsFromAssembly(
            typeof(IAssemblyMarker).Assembly,
            includeInternalTypes: true);

        return services;
    }
}