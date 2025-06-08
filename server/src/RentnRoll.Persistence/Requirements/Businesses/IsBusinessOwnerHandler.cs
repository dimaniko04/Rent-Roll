using System.Security.Claims;

using Microsoft.AspNetCore.Authorization;

using RentnRoll.Application.Common.Interfaces.Repositories;
using RentnRoll.Application.Common.Interfaces.UnitOfWork;

namespace RentnRoll.Persistence.Requirements.Businesses;

public class IsBusinessOwnerHandler
    : AuthorizationHandler<IsBusinessOwnerRequirement, Guid>
{
    private readonly IBusinessRepository _businessRepository;

    public IsBusinessOwnerHandler(
        IUnitOfWork unitOfWork)
    {
        _businessRepository = unitOfWork
            .GetRepository<IBusinessRepository>();
    }

    protected override async Task HandleRequirementAsync(
        AuthorizationHandlerContext context,
        IsBusinessOwnerRequirement requirement,
        Guid businessId)
    {
        var business = await _businessRepository
            .GetByIdAsync(businessId);

        if (business == null)
        {
            context.Fail(new AuthorizationFailureReason(
                this, "Business does not exist."));
            return;
        }

        var userId = context
            .User
            .FindFirstValue(ClaimTypes.NameIdentifier);

        if (userId == null ||
            business.OwnerId != userId)
        {
            context.Fail(new AuthorizationFailureReason(
                this, "User is not the owner of the business."));
            return;
        }

        context.Succeed(requirement);
        return;
    }
}