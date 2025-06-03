using System.Security.Claims;

using Microsoft.AspNetCore.Http;

namespace RentnRoll.Application.Common.UserContext;

public class CurrentUserContext : ICurrentUserContext
{
    private readonly IHttpContextAccessor _accessor;

    public CurrentUserContext(IHttpContextAccessor accessor)
    {
        _accessor = accessor;
    }

    public string UserId =>
        _accessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value
        ?? throw new InvalidOperationException(
            "User ID not found in the current context.");

    public List<string> Roles =>
        _accessor.HttpContext?.User?.FindAll(ClaimTypes.Role)
            .Select(c => c.Value)
            .ToList()
        ?? throw new InvalidOperationException(
            "Roles not found in the current context.");
}