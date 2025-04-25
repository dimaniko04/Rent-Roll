using FluentValidation;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

using RentnRoll.Domain.Common;

namespace RentnRoll.Application.Services.Validation;

public class ValidationService : IValidationService
{
    private readonly ILogger<ValidationService> _logger;
    private readonly IServiceProvider _serviceProvider;

    public ValidationService(
        ILogger<ValidationService> logger,
        IServiceProvider serviceProvider)
    {
        _logger = logger;
        _serviceProvider = serviceProvider;
    }

    public async Task<Result<T>> ValidateAsync<T>(T instance)
    {
        var validator = _serviceProvider
            .GetService<IValidator<T>>();

        if (validator is null)
        {
            return instance;
        }

        var validationResult = await validator
            .ValidateAsync(instance);

        if (validationResult.IsValid)
        {
            return instance;
        }

        _logger.LogError(
                "Validation Error: Validation - One or more validation errors occurred - {@ValidationResult}",
                validationResult.Errors);

        var errors = validationResult
            .Errors
            .ConvertAll(error => Error.Validation(
                error.PropertyName,
                error.ErrorMessage));

        return errors;
    }
}