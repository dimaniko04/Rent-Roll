using RentnRoll.Application.Contracts.Common;
using RentnRoll.Domain.Entities.PricingPolicies.Enums;

namespace RentnRoll.Application.Contracts.PricingPolicies.GetAllPricingPolicies;

public record GetAllPricingPoliciesRequest(
    TimeUnit? TimeUnit,
    bool EmptyOnly = false
) : QueryParams;