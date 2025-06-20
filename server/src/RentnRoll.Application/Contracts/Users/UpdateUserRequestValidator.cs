using FluentValidation;

namespace RentnRoll.Application.Contracts.Users;

public class UpdateUserRequestValidator
    : AbstractValidator<UpdateUserRequest>
{
    public UpdateUserRequestValidator()
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
    }
}