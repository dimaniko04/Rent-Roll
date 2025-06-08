using System.Security.Claims;

using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;

using RentnRoll.Application.Common.Interfaces.Repositories;
using RentnRoll.Application.Common.Interfaces.UnitOfWork;
using RentnRoll.Domain.Constants;

namespace RentnRoll.Persistence.Requirements.Businesses;

public class IsBusinessOwnerOrAdminHandler
    : AuthorizationHandler<IsBusinessOwnerOrAdminRequirement, Guid>
{
    private readonly ILogger<IsBusinessOwnerOrAdminHandler> _logger;
    private readonly IBusinessRepository _businessRepository;

    public IsBusinessOwnerOrAdminHandler(
        IUnitOfWork unitOfWork,
        ILogger<IsBusinessOwnerOrAdminHandler> logger)
    {
        _logger = logger;
        _businessRepository = unitOfWork
            .GetRepository<IBusinessRepository>();
    }

    protected override async Task HandleRequirementAsync(
        AuthorizationHandlerContext context,
        IsBusinessOwnerOrAdminRequirement requirement,
        Guid businessId)
    {
        var isAdmin = context.User
            .IsInRole(Roles.Admin);

        _logger.LogDebug("Checking if user is admin: {IsAdmin}", isAdmin);

        if (isAdmin)
        {
            context.Succeed(requirement);
            return;
        }

        var business = await _businessRepository
            .GetByIdAsync(businessId);

        _logger.LogDebug("Business retrieved: {BusinessId}", business?.Id);

        if (business == null)
        {
            context.Fail(new AuthorizationFailureReason(
                this, "Business does not exist."));
            return;
        }

        var userId = context.User.FindFirstValue(ClaimTypes.NameIdentifier);

        _logger.LogDebug("User ID from context: {UserId}", userId);

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