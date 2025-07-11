using System.Security.Claims;

using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;

using RentnRoll.Application.Common.Interfaces.Repositories;
using RentnRoll.Application.Common.Interfaces.UnitOfWork;
using RentnRoll.Application.Contracts.Lockers.AssignGames;
using RentnRoll.Domain.Constants;

namespace RentnRoll.Persistence.Requirements.Cells;

public class IsAssignedBusinessOwnerHandler
    : AuthorizationHandler<
        IsAssignedBusinessOwnerRequirement,
        AssignGamesRequest>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<IsAssignedBusinessOwnerHandler> _logger;

    public IsAssignedBusinessOwnerHandler(
        IUnitOfWork unitOfWork,
        ILogger<IsAssignedBusinessOwnerHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    protected override async Task HandleRequirementAsync(
        AuthorizationHandlerContext context,
        IsAssignedBusinessOwnerRequirement requirement,
        AssignGamesRequest resource)
    {
        var cellIds = resource
            .GameAssignments
            .Select(ga => ga.CellId)
            .ToList();

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

        _logger.LogDebug(
            "User {UserId} is a business owner, proceeding with authorization check.",
            userId);

        var business = await _unitOfWork
            .GetRepository<IBusinessRepository>()
            .GetByOwnerIdAsync(userId);

        if (business == null)
        {
            context.Fail(new AuthorizationFailureReason(
                this, "User does not have a registered business."));
            return;
        }

        _logger.LogDebug(
            "Business {BusinessId} found for user {UserId}, checking locker and cells.",
            business.Id, userId);

        var cells = await _unitOfWork
            .GetRepository<ILockerRepository>()
            .GetCellsByIdsAsync(cellIds);

        if (cells.Any(c => c.BusinessId != business.Id))
        {
            context.Fail(new AuthorizationFailureReason(
                this, "Not all cells are assigned to the business."));
            return;
        }

        _logger.LogDebug(
            "All cells are assigned to the business {BusinessId}, authorization successful.",
            business.Id);

        context.Succeed(requirement);
        return;
    }
}