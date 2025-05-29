using FluentValidation;

namespace RentnRoll.Application.Contracts.Mechanics;

public class UpdateMechanicRequestValidator
    : AbstractValidator<UpdateMechanicRequest>
{
    public UpdateMechanicRequestValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("Name is required.")
            .MaximumLength(200)
            .WithMessage("Name must not exceed 200 characters.");
    }
}