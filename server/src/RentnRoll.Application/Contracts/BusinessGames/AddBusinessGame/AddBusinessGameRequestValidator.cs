using FluentValidation;

using RentnRoll.Application.Common.Interfaces.Repositories;
using RentnRoll.Application.Common.Interfaces.UnitOfWork;


namespace RentnRoll.Application.Contracts.BusinessGames.AddBusinessGame;

public class AddBusinessGameRequestValidator
    : AbstractValidator<AddBusinessGameRequest>
{
    public AddBusinessGameRequestValidator(IUnitOfWork unitOfWork)
    {
        RuleFor(x => x.GameId)
            .NotEmpty()
            .WithMessage("Game is required.")
            .MustAsync(async (gameId, cancellation) =>
            {
                var repository = unitOfWork
                    .GetRepository<IGameRepository>();
                var game = await repository.GetByIdAsync(gameId);
                return game != null;
            })
            .WithMessage("Game does not exist.");

        RuleFor(x => x.Quantity)
            .GreaterThan(0)
            .WithMessage("Quantity must be greater than 0.")
            .LessThanOrEqualTo(100000)
            .WithMessage("Quantity must not exceed 100000.");

        RuleFor(x => x.BasePrice)
            .GreaterThanOrEqualTo(0)
            .WithMessage("Base price must be a non-negative value.")
            .LessThanOrEqualTo(100000)
            .WithMessage("Base price must not exceed 1000000.");

        RuleFor(x => x.Tags)
            .NotEmpty()
            .When(x => x.Tags is not null)
            .WithMessage("At least one tag is required.")
            .Must(tags => tags?.Count <= 10)
            .When(x => x.Tags is not null)
            .WithMessage("You can add a maximum of 10 tags.");
    }
}