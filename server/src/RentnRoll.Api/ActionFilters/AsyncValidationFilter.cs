using FluentValidation;
using FluentValidation.Results;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace RentnRoll.Api.ActionFilters;

public class AsyncValidationFilter : IAsyncActionFilter
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<AsyncValidationFilter> _logger;

    public AsyncValidationFilter(
        IServiceProvider serviceProvider,
        ILogger<AsyncValidationFilter> logger)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
    }

    public async Task OnActionExecutionAsync(
        ActionExecutingContext context,
        ActionExecutionDelegate next)
    {
        var actionArgumentsValues = context.ActionArguments
            .Select(a => a.Value);

        foreach (var actionArgumentValue in actionArgumentsValues)
        {
            _logger.LogDebug(
                "Validating action argument: {@ActionArgumentValue}",
                actionArgumentValue);

            if (actionArgumentValue is null)
            {
                continue;
            }

            var type = actionArgumentValue.GetType();
            var validator = _serviceProvider
                .GetService(typeof(IValidator<>)
                .MakeGenericType(type)) as IValidator;

            if (validator is null)
            {
                continue;
            }

            var validationResult = await validator
                .ValidateAsync(
                    new ValidationContext<object>(actionArgumentValue),
                    context.HttpContext.RequestAborted);

            if (!validationResult.IsValid)
            {
                onValidationError(context, validationResult);
                return;
            }
        }

        await next();
    }

    private void onValidationError(
        ActionExecutingContext context,
        ValidationResult validationResult)
    {
        var problemDetails = new ValidationProblemDetails(
            validationResult.ToDictionary())
        {
            Type = "https://httpstatuses.com/409",
            Status = StatusCodes.Status409Conflict,
            Title = "Validation failed",
            Detail = "One or more validation errors occurred",
            Instance = context.HttpContext.Request.Path
        };

        _logger.LogError(
                "Validation Error: Conflict - {@Message} - {@ValidationResult}",
                problemDetails.Detail,
                validationResult.Errors);

        context.Result = new ConflictObjectResult(problemDetails);
    }
}