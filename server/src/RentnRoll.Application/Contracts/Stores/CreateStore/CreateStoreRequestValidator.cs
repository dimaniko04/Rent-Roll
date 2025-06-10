using FluentValidation;

using RentnRoll.Application.Validators;

namespace RentnRoll.Application.Contracts.Stores.CreateStore;

public class CreateStoreRequestValidator : AbstractValidator<CreateStoreRequest>
{
    public CreateStoreRequestValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("Store name is required.")
            .MaximumLength(200)
            .WithMessage("Store name must not exceed 200 characters.");

        RuleFor(x => x.Address)
            .NotNull()
            .WithMessage("Address is required.");

        RuleFor(x => x.Address)
            .SetValidator(new AddressValidator())
            .When(x => x.Address is not null);
    }
}