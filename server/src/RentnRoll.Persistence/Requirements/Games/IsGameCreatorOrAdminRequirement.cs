using Microsoft.AspNetCore.Authorization;

namespace RentnRoll.Persistence.Requirements.Games;

public class IsGameCreatorOrAdminRequirement : IAuthorizationRequirement
{
}