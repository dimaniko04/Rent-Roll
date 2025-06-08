namespace RentnRoll.Application.Contracts.PricingPolicies.UpdatePricingPolicy;

public record UpdatePricingPolicyItemRequest(
    Guid BusinessGameId,
    int Price);
