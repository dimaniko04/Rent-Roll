using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace RentnRoll.API.Middlewares;

internal sealed class GlobalExceptionHandler : IExceptionHandler
{
    private readonly ILogger<GlobalExceptionHandler> _logger;

    public GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger)
    {
        _logger = logger;
    }

    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {
        _logger.LogError(
            exception,
            "An unhandled exception occurred: {Message} {@Exception}",
            exception.Message,
            exception);

        var problemDetails = new ProblemDetails
        {
            Type = "https://httpstatuses.com/500",
            Title = "An unexpected error occurred",
            Status = StatusCodes.Status500InternalServerError,
            Detail = exception.Message,
            Instance = httpContext.Request.Path
        };
        httpContext.Response.ContentType = "application/problem+json";
        httpContext.Response.StatusCode = problemDetails.Status.Value;

        await httpContext.Response
            .WriteAsJsonAsync(problemDetails, cancellationToken);

        return true;
    }
}