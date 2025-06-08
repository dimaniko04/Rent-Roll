using FluentValidation;

using RentnRoll.Application.Contracts.PricingPolicies.CreatePricingPolicy;

namespace RentnRoll.Application.Contracts.PricingPolicies.UpdatePricingPolicy;

public class UpdatePricingPolicyRequestValidator
    : AbstractValidator<UpdatePricingPolicyRequest>
{
    public UpdatePricingPolicyRequestValidator()
    {
        RuleFor(x => x)
            .SetValidator(
                new CreatePricingPolicyRequestValidator());

        RuleFor(x => x.Items)
            .NotEmpty()
            .WithMessage("At least one game must be attached to policy item list.")
            .When(x => x.Items is not null);

        RuleForEach(x => x.Items)
            .SetValidator(new UpdatePricingPolicyItemRequestValidator())
            .When(x => x.Items is not null);
    }
}