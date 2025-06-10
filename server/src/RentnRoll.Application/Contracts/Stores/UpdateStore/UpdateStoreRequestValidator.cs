using FluentValidation;

using RentnRoll.Application.Validators;

namespace RentnRoll.Application.Contracts.Stores.UpdateStore;

public class UpdateStoreRequestValidator : AbstractValidator<UpdateStoreRequest>
{
    public UpdateStoreRequestValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("Store name is required.")
            .MaximumLength(200)
            .WithMessage("Store name cannot exceed 200 characters.");

        RuleFor(x => x.Address)
            .NotNull()
            .WithMessage("Address is required.");

        RuleFor(x => x.Address)
            .SetValidator(new AddressValidator())
            .When(x => x.Address is not null);

        RuleFor(x => x.Assets)
            .NotEmpty()
            .WithMessage("At least one asset must be attached to the store.")
            .When(x => x.Assets is not null);

        RuleForEach(x => x.Assets)
            .ChildRules(x =>
            {
                x.RuleFor(asset => asset.BusinessGameId)
                    .NotEmpty()
                    .WithMessage("Business game id is required.");

                x.RuleFor(asset => asset.Quantity)
                    .GreaterThan(0)
                    .WithMessage("Quantity must be greater than zero.")
                    .LessThan(1000)
                    .WithMessage("Quantity must be less than 1000.");
            })
            .When(x => x.Assets is not null);
    }
}