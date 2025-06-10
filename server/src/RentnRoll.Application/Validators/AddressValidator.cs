using FluentValidation;

using RentnRoll.Domain.ValueObjects;

namespace RentnRoll.Application.Validators;

public class AddressValidator : AbstractValidator<Address>
{
    public AddressValidator()
    {
        RuleFor(x => x.Street)
            .NotEmpty()
            .WithMessage("Street address is required.")
            .MaximumLength(200)
            .WithMessage("Street address must not exceed 200 characters.");

        RuleFor(x => x.City)
            .NotEmpty()
            .WithMessage("City is required.")
            .MaximumLength(100)
            .WithMessage("City name must not exceed 100 characters.");

        RuleFor(x => x.State)
            .NotEmpty()
            .WithMessage("State is required.")
            .MaximumLength(100)
            .WithMessage("State must not exceed 100 characters.");

        RuleFor(x => x.Country)
            .NotEmpty()
            .WithMessage("Country is required.")
            .MaximumLength(100)
            .WithMessage("Country must not exceed 100 characters.");

        RuleFor(x => x.ZipCode)
            .NotEmpty()
            .WithMessage("Zip code is required.")
            .MaximumLength(20)
            .WithMessage("Zip code must not exceed 20 characters.");
    }
}
