namespace RentnRoll.Application.Contracts.Lockers.AssignGames;

public record GameAssignment(
    Guid? BusinessGameId,
    Guid CellId
);
