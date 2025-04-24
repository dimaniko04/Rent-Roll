using Microsoft.AspNetCore.Mvc;

using RentnRoll.Core.Common;

namespace RentnRoll.API.Controllers;

public class ApiController : ControllerBase
{
    protected IActionResult Problem(Error error)
    {
        var (type, statusCode) = error.Type switch
        {
            ErrorType.NotFound => (
                "https://httpstatuses.com/404",
                StatusCodes.Status404NotFound
            ),
            ErrorType.Forbidden => (
                "https://httpstatuses.com/403",
                StatusCodes.Status403Forbidden
            ),
            ErrorType.InvalidRequest => (
                "https://httpstatuses.com/400",
                StatusCodes.Status400BadRequest
            ),
            ErrorType.Unauthorized => (
                "https://httpstatuses.com/401",
                StatusCodes.Status401Unauthorized
            ),
            _ => (
                "https://httpstatuses.com/500",
                StatusCodes.Status500InternalServerError
            )
        };

        return Problem(
            type: type,
            title: error.Message,
            detail: error.Details,
            statusCode: statusCode,
            instance: HttpContext.Request.Path
        );
    }
}