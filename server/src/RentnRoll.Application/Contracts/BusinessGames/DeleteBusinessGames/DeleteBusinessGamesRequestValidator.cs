using FluentValidation;

namespace RentnRoll.Application.Contracts.BusinessGames.DeleteBusinessGames;

public class DeleteBusinessGamesRequestValidator
    : AbstractValidator<DeleteBusinessGamesRequest>
{
    public DeleteBusinessGamesRequestValidator()
    {
        RuleFor(x => x.Ids)
            .NotEmpty()
            .WithMessage("List of games to delete must be provided.");
    }
}