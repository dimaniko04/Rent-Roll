using System.Security.Claims;

using Microsoft.AspNetCore.Authorization;

using RentnRoll.Domain.Constants;
using RentnRoll.Domain.Entities.Games;

namespace RentnRoll.Persistence.Requirements;

public class IsGameCreatorOrAdminHandler
    : AuthorizationHandler<IsGameCreatorOrAdminRequirement, Game>
{
    protected override Task HandleRequirementAsync(
        AuthorizationHandlerContext context,
        IsGameCreatorOrAdminRequirement requirement,
        Game resource)
    {
        if (context.User.IsInRole(Roles.Admin) ||
            context.User.FindFirstValue(ClaimTypes.NameIdentifier) ==
            resource.CreatedByUserId)
        {
            context.Succeed(requirement);
        }
        return Task.CompletedTask;
    }
}