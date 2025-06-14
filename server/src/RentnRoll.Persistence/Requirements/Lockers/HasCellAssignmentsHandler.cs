using System.Security.Claims;

using Microsoft.AspNetCore.Authorization;

using RentnRoll.Application.Common.Interfaces.Repositories;
using RentnRoll.Application.Common.Interfaces.UnitOfWork;
using RentnRoll.Domain.Constants;

namespace RentnRoll.Persistence.Requirements.Lockers;

public class HasCellAssignmentsHandler
    : AuthorizationHandler<HasCellAssignmentsRequirement, Guid>
{
    private readonly IUnitOfWork _unitOfWork;

    public HasCellAssignmentsHandler(
        IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    protected override async Task HandleRequirementAsync(
        AuthorizationHandlerContext context,
        HasCellAssignmentsRequirement requirement,
        Guid lockerId
    )
    {
        var isOwner = context
            .User
            .IsInRole(Roles.Business);
        var userId = context
            .User
            .FindFirstValue(ClaimTypes.NameIdentifier);

        if (userId == null)
        {
            context.Fail(new AuthorizationFailureReason(
                this, "User is not authenticated."));
            return;
        }
        if (!isOwner)
        {
            context.Fail(new AuthorizationFailureReason(
                this, "User is not a business owner."));
            return;
        }

        var business = await _unitOfWork
            .GetRepository<IBusinessRepository>()
            .GetByOwnerIdAsync(userId);

        if (business == null)
        {
            context.Fail(new AuthorizationFailureReason(
                this, "User does not have a registered business."));
            return;
        }

        var locker = await _unitOfWork
            .GetRepository<ILockerRepository>()
            .GetByIdAsync(lockerId);

        if (locker == null)
        {
            context.Fail(new AuthorizationFailureReason(
                this, "Locker does not exist."));
            return;
        }

        var hasAssignment = locker.Cells
            .Any(c => c.BusinessId == business.Id);

        if (!hasAssignment)
        {
            context.Fail(new AuthorizationFailureReason(
                this, "Locker does not have any cell assignments for the business."));
            return;
        }

        context.Succeed(requirement);
        return;
    }
}