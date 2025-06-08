using Microsoft.AspNetCore.Authorization;

namespace RentnRoll.Persistence.Requirements.Businesses;

public class IsBusinessOwnerOrAdminRequirement
    : IAuthorizationRequirement
{
}