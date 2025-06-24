using FluentValidation;

namespace RentnRoll.Application.Contracts.Lockers.AssignGames;

public class AssignGamesRequestValidator
    : AbstractValidator<AssignGamesRequest>
{
    public AssignGamesRequestValidator()
    {
        RuleFor(x => x.GameAssignments)
            .NotEmpty()
            .WithMessage("Game assignments cannot be empty.");

        RuleForEach(x => x.GameAssignments)
            .ChildRules(gameAssignment =>
            {
                gameAssignment.RuleFor(x => x.CellId)
                    .NotEmpty()
                    .WithMessage("Cell ID cannot be empty.");
            })
            .When(x => x.GameAssignments is not null && x.GameAssignments.Count != 0);
    }
}