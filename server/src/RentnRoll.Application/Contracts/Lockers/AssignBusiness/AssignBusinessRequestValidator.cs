using FluentValidation;

namespace RentnRoll.Application.Contracts.Lockers.AssignBusiness;

public class AssignBusinessRequestValidator
    : AbstractValidator<AssignBusinessRequest>
{
    public AssignBusinessRequestValidator()
    {
        RuleFor(x => x.BusinessId)
            .NotEmpty()
            .WithMessage("Business ID is required.");

        RuleFor(x => x.CellCount)
            .GreaterThan(0)
            .WithMessage("Cell count must be greater than zero.")
            .LessThanOrEqualTo(100)
            .WithMessage("Cell count must not exceed 100.");
    }
}