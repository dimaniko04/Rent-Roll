using FluentValidation;

using RentnRoll.Application.Validators;
using RentnRoll.Domain.Entities.PricingPolicies.Enums;
using RentnRoll.Domain.Enums;

namespace RentnRoll.Application.Contracts.Rentals.CreateRental;

public class CreateRentalRequestValidator
    : AbstractValidator<CreateRentalRequest>
{
    public CreateRentalRequestValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Either store asset or cell id must be provided.");

        RuleFor(x => x.Type)
            .NotEmpty()
            .WithMessage("Rental type must either \"Locker\" or \"GameStore\".")
            .IsEnumName<CreateRentalRequest, LocationType>()
            .WithMessage("Rental type must either \"Locker\" or \"GameStore\".");

        RuleFor(x => x.Unit)
            .NotEmpty()
            .WithMessage("Rental unit is required e.g. \"Hour\", \"Day\" etc.")
            .IsEnumName<CreateRentalRequest, TimeUnit>()
            .WithMessage("Unsupported unit.");

        RuleFor(x => x.Term)
            .GreaterThan(0)
            .WithMessage("Term must be greater than zero.");
    }
}