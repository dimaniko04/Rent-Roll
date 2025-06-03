using FluentValidation;

namespace RentnRoll.Application.Contracts.Games.ReplaceGameImages;

public class ReplaceGameImagesRequestValidator
    : AbstractValidator<ReplaceGameImagesRequest>
{
    public ReplaceGameImagesRequestValidator()
    {
        RuleFor(x => x.Files)
            .NotEmpty()
            .WithMessage("At least one file is required.")
            .NotEmpty()
            .WithMessage("Files collection must not be empty.");

        RuleFor(x => x.UnmodifiedImagePaths)
            .NotEmpty()
            .When(x => x.UnmodifiedImagePaths is not null)
            .WithMessage("List of unmodified image paths must not be empty if provided.");
    }
}