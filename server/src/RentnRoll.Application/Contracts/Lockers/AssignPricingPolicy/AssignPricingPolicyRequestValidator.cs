using FluentValidation;

namespace RentnRoll.Application.Contracts.Lockers.AssignPricingPolicy;

public class AssignPricingPolicyRequestValidator
    : AbstractValidator<AssignPricingPolicyRequest>
{
    public AssignPricingPolicyRequestValidator()
    {
        RuleFor(x => x.PricingPolicyId)
            .NotEmpty()
            .WithMessage("Pricing policy ID is required.");
    }
}