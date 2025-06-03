using FluentValidation;


using RentnRoll.Application.Common.Interfaces.UnitOfWork;
using RentnRoll.Application.Contracts.Games.CreateGame;

namespace RentnRoll.Application.Contracts.Games.UpdateGame;

public class UpdateGameValidator
    : AbstractValidator<UpdateGameRequest>
{
    public UpdateGameValidator(IUnitOfWork unitOfWork)
    {
        RuleFor(x => x)
            .SetValidator(new CreateGameRequestValidator(
                unitOfWork,
                skipDuplicateCheck: true));

        RuleFor(x => x.AveragePlayTime)
            .GreaterThanOrEqualTo(0)
            .When(x => x.AveragePlayTime.HasValue)
            .WithMessage("Average play time must be a non-negative value.")
            .LessThanOrEqualTo(24 * 60)
            .When(x => x.AveragePlayTime.HasValue)
            .WithMessage("Average play time must not exceed 24 hours in minutes.");

        RuleFor(x => x.ComplexityScore)
            .GreaterThanOrEqualTo(100)
            .When(x => x.ComplexityScore.HasValue)
            .WithMessage("Complexity score must be greater than or equal to 100.")
            .LessThanOrEqualTo(500)
            .When(x => x.ComplexityScore.HasValue)
            .WithMessage("Complexity score must not exceed 500.");

        RuleFor(x => x.Genres)
            .NotEmpty()
            .When(x => x.Genres is not null)
            .WithMessage("At least one genre is required.");

        RuleFor(x => x.Categories)
            .NotEmpty()
            .When(x => x.Categories is not null)
            .WithMessage("At least one category is required.");

        RuleFor(x => x.Mechanics)
            .NotEmpty()
            .When(x => x.Mechanics is not null)
            .WithMessage("At least one mechanic is required.");
    }
}