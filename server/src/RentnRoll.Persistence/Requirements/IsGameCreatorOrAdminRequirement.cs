using Microsoft.AspNetCore.Authorization;

namespace RentnRoll.Persistence.Requirements;

public class IsGameCreatorOrAdminRequirement : IAuthorizationRequirement
{
}