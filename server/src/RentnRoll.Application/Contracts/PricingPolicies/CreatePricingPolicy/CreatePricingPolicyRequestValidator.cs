using FluentValidation;

using RentnRoll.Application.Validators;
using RentnRoll.Domain.Entities.PricingPolicies.Enums;

namespace RentnRoll.Application.Contracts.PricingPolicies.CreatePricingPolicy;

public class CreatePricingPolicyRequestValidator
    : AbstractValidator<CreatePricingPolicyRequest>
{
    public CreatePricingPolicyRequestValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("Name is required.")
            .MaximumLength(200)
            .WithMessage("Name must not exceed 200 characters.");

        RuleFor(x => x.Description)
            .NotEmpty()
            .WithMessage("Description is required.")
            .MaximumLength(500)
            .WithMessage("Description must not exceed 500 characters.");

        RuleFor(x => x.TimeUnit)
            .IsEnumName<CreatePricingPolicyRequest, TimeUnit>();

        RuleFor(x => x.UnitCount)
            .GreaterThan(0)
            .WithMessage("Unit count must be greater than 0.");

        RuleFor(x => x.PricePercent)
            .InclusiveBetween(0, 100)
            .WithMessage("Price percent must be between 0 and 100.");
    }
}