using FluentValidation;

using RentnRoll.Application.Validators;

namespace RentnRoll.Application.Contracts.Users;

public class UpdateUserRequestValidator
    : AbstractValidator<UpdateUserRequest>
{
    public UpdateUserRequestValidator()
    {
        RuleFor(x => x.FirstName)
            .NotEmpty()
            .WithMessage("First name is required.")
            .MaximumLength(100)
            .WithMessage("First name must not exceed 100 characters.");

        RuleFor(x => x.LastName)
            .NotEmpty()
            .WithMessage("Last name is required.")
            .MaximumLength(200)
            .WithMessage("Last name must not exceed 200 characters.");

        RuleFor(x => x.Email)
            .NotEmpty()
            .WithMessage("Email is required.")
            .EmailAddress()
            .WithMessage("Invalid email format.");


        RuleFor(x => x.BirthDate)
            .NotEmpty()
            .WithMessage("Birth date is required.")
            .Must(date => date <= DateTime.UtcNow)
            .WithMessage("Birth date must be in the past.")
            .GreaterThan(DateTime.UtcNow.AddYears(-80))
            .WithMessage("Birth date must be within the last 80 years.");

        RuleFor(x => x.Country)
            .NotEmpty()
            .WithMessage("Country is required.");

        RuleFor(x => x.PhoneNumber)
            .NotEmpty()
            .WithMessage("User phone number is required.")
            .PhoneNumber()
            .WithMessage("Invalid phone number format.");
    }
}