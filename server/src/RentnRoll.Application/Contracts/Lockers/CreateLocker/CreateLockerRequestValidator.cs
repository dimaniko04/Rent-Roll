using FluentValidation;

using RentnRoll.Application.Validators;

namespace RentnRoll.Application.Contracts.Lockers.CreateLocker;

public class CreateLockerRequestValidator
    : AbstractValidator<CreateLockerRequest>
{
    public CreateLockerRequestValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("Locker name is required.");

        RuleFor(x => x.NumberOfCells)
            .GreaterThan(0)
            .WithMessage("Number of cells must be greater than zero.")
            .LessThanOrEqualTo(100)
            .WithMessage("Number of cells must not exceed 100.");

        RuleFor(x => x.Address)
            .NotNull()
            .WithMessage("Address is required.");

        RuleFor(x => x.Address)
            .SetValidator(new AddressValidator())
            .When(x => x.Address is not null);
    }
}