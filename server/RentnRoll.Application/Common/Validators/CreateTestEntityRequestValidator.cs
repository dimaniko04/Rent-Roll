using FluentValidation;

using RentnRoll.Application.Common.Request;

namespace RentnRoll.Application.Common.Validators;

internal sealed class CreateTestEntityRequestValidator
    : AbstractValidator<CreateTestEntityRequest>
{
    public CreateTestEntityRequestValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("Name is required.")
            .MaximumLength(100)
            .WithMessage("Name must not exceed 100 characters.");

        RuleFor(x => x.Secret)
            .NotEmpty()
            .WithMessage("Secret is required.")
            .MinimumLength(8)
            .WithMessage("Secret must be at least 8 characters long.")
            .MaximumLength(20)
            .WithMessage("Secret must not exceed 20 characters.");
    }
}