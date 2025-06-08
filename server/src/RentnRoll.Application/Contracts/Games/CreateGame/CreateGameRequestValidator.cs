using FluentValidation;

using RentnRoll.Application.Common.Interfaces.Repositories;
using RentnRoll.Application.Common.Interfaces.UnitOfWork;
using RentnRoll.Application.Specifications.Games;

namespace RentnRoll.Application.Contracts.Games.CreateGame;

public class CreateGameRequestValidator
    : AbstractValidator<CreateGameRequest>
{
    public CreateGameRequestValidator(
        IUnitOfWork unitOfWork,
        bool skipDuplicateCheck = false)
    {
        var repository = unitOfWork
            .GetRepository<IGameRepository>();

        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("Name is required.")
            .MaximumLength(300)
            .WithMessage("Name must not exceed 300 characters.");

        RuleFor(x => x.Name)
            .MustAsync(async (name, cancellation) =>
            {
                var specification = new GameNameSpec(name);
                var exists = await repository
                    .GetSingleAsync(specification);
                return exists == null;
            })
            .When(x => !skipDuplicateCheck)
            .WithMessage("A game with this name already exists.");

        RuleFor(x => x.Description)
            .NotEmpty()
            .WithMessage("Description is required.")
            .MaximumLength(2000)
            .WithMessage("Description must not exceed 2000 characters.");

        RuleFor(x => x.PublishedAt)
            .NotEmpty()
            .WithMessage("Publication date is required.")
            .LessThan(DateTime.UtcNow)
            .WithMessage("Publication date must be in the past.")
            .GreaterThan(DateTime.UtcNow.AddYears(-80))
            .WithMessage("Publication date must be within the last 80 years.");

        RuleFor(x => x.MinPlayers)
            .GreaterThan(0)
            .WithMessage("The minimum number of players must be greater than 0.")
            .LessThanOrEqualTo(10)
            .WithMessage("The minimum number of players must not exceed 10.");

        RuleFor(x => x.MaxPlayers)
            .GreaterThan(0)
            .WithMessage("The maximum number of players must be greater than 0.")
            .LessThanOrEqualTo(100)
            .WithMessage("The maximum number of players must not exceed 100.")
            .GreaterThan(x => x.MinPlayers)
            .WithMessage("The maximum number of players must be greater than the minimum number of players.");

        RuleFor(x => x.Age)
            .GreaterThanOrEqualTo(0)
            .WithMessage("The age rating must be greater than or equal to 0.")
            .LessThanOrEqualTo(18)
            .WithMessage("The age rating must not exceed 18.");
    }
}