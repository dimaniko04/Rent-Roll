using FluentValidation;

using RentnRoll.Application.Validators;

namespace RentnRoll.Application.Contracts.Authentication;

internal sealed class UserRegisterRequestValidator
    : AbstractValidator<UserRegisterRequest>
{
    public UserRegisterRequestValidator()
    {
        RuleFor(x => x.FullName)
            .NotEmpty()
            .WithMessage("User full name is required.")
            .MaximumLength(400)
            .WithMessage("User full name must not exceed 400 characters.");

        RuleFor(x => x.Email)
            .NotEmpty()
            .WithMessage("Email is required.")
            .EmailAddress()
            .WithMessage("Invalid email format.");

        RuleFor(x => x.Country)
            .NotEmpty()
            .WithMessage("Country is required.");

        RuleFor(x => x.Password)
            .NotEmpty()
            .WithMessage("Password is required.")
            .MinimumLength(8)
            .WithMessage("Password must be at least 8 characters long.")
            .MaximumLength(100)
            .WithMessage("Password must not exceed 100 characters.");
    }
}