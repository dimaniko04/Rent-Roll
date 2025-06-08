using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

using RentnRoll.Domain.Common;

namespace RentnRoll.Api.Controllers;

public class ApiController : ControllerBase
{


    protected async Task<Result> AuthorizeForResource(
        object? resource,
        string policy)
    {
        var authorizationService = HttpContext
            .RequestServices
            .GetService(typeof(IAuthorizationService))
            as IAuthorizationService;

        var authorizationResult = await authorizationService!
            .AuthorizeAsync(User, resource, policy);

        if (!authorizationResult.Succeeded)
        {
            var errorMessages = authorizationResult.Failure
                .FailureReasons
                .Select(reason => reason.Message)
                .ToList();
            return Result.Failure([
                Error.Forbidden(
                    "Authorization.Forbidden",
                    $"You are not authorized to access this resource. " +
                    $"{string.Join(", ", errorMessages)}"
                )]);
        }

        return Result.Success();
    }


    protected IActionResult Problem(List<Error> errors)
    {
        if (errors.All(e => e.Type == ErrorType.Validation))
        {
            return ValidationProblem(errors);
        }

        return Problem(errors.First());
    }

    private IActionResult Problem(Error error)
    {
        var statusCode = error.Type switch
        {
            ErrorType.NotFound => StatusCodes.Status404NotFound,
            ErrorType.Forbidden => StatusCodes.Status403Forbidden,
            ErrorType.InvalidRequest => StatusCodes.Status400BadRequest,
            ErrorType.Unauthorized => StatusCodes.Status401Unauthorized,
            ErrorType.Unsupported => StatusCodes.Status415UnsupportedMediaType,
            _ => StatusCodes.Status500InternalServerError
        };

        return Problem(
            type: $"https://httpstatuses.com/{statusCode}",
            title: error.Message,
            detail: error.Details,
            statusCode: statusCode,
            instance: HttpContext.Request.Path
        );
    }

    private IActionResult ValidationProblem(
        List<Error> errors)
    {
        var modelStateDictionary = new ModelStateDictionary();

        errors.ForEach(error => modelStateDictionary
            .AddModelError(error.Code, error.Message));

        return ValidationProblem(modelStateDictionary);
    }
}