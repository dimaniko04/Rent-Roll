using FluentValidation;

namespace RentnRoll.Application.Contracts.PricingPolicies.UpdatePricingPolicy;

public class UpdatePricingPolicyItemRequestValidator
    : AbstractValidator<UpdatePricingPolicyItemRequest>
{
    public UpdatePricingPolicyItemRequestValidator()
    {
        RuleFor(x => x.BusinessGameId)
            .NotEmpty()
            .WithMessage("Business game ID must not be empty.");

        RuleFor(x => x.Price)
            .GreaterThan(0)
            .WithMessage("Price must be greater than zero.");
    }
}