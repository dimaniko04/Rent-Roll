using FluentValidation;

namespace RentnRoll.Application.Contracts.Rentals.CreateRental;

public class CreateRentalRequestValidator
    : AbstractValidator<CreateRentalRequest>
{
    public CreateRentalRequestValidator()
    {
        RuleFor(x => x.StoreAssetId)
            .NotEmpty()
            .WithMessage("Either cell id or store game id must be provided.")
            .When(x => x.CellId == Guid.Empty, ApplyConditionTo.CurrentValidator);

        RuleFor(x => x.CellId)
            .NotEmpty()
            .WithMessage("Either cell id or store game id must be provided.")
            .When(x => x.StoreAssetId == Guid.Empty, ApplyConditionTo.CurrentValidator);

        RuleFor(x => x.StoreAssetId)
            .Empty()
            .WithMessage("Cell id and store game id can't be both specified.")
            .When(x => x.CellId != Guid.Empty, ApplyConditionTo.CurrentValidator);

        RuleFor(x => x.CellId)
            .Empty()
            .WithMessage("Cell id and store game id can't be both specified.")
            .When(x => x.StoreAssetId != Guid.Empty, ApplyConditionTo.CurrentValidator);

        RuleFor(x => x.Term)
            .GreaterThan(0)
            .WithMessage("Term must be greater than zero.");
    }
}