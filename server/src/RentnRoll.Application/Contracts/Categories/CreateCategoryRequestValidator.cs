using FluentValidation;

namespace RentnRoll.Application.Contracts.Categories;

public class CreateCategoryRequestValidator
    : AbstractValidator<CreateCategoryRequest>
{
    public CreateCategoryRequestValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("Name is required.")
            .MaximumLength(200)
            .WithMessage("Name must not exceed 200 characters.");
    }
}