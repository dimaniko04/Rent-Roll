using RentnRoll.Domain.Entities.PricingPolicies;
using RentnRoll.Domain.Entities.PricingPolicies.Enums;

namespace RentnRoll.Application.Common.Interfaces.Repositories;

public interface IPricingPolicyRepository : IBaseRepository<PricingPolicy>
{
    Task<PricingPolicyItem?> GetCellPolicyAsync(
        Guid cellId,
        TimeUnit unit);

    Task<PricingPolicyItem?> GetAssetPolicyAsync(
        Guid storeAssetId,
        TimeUnit unit);
}