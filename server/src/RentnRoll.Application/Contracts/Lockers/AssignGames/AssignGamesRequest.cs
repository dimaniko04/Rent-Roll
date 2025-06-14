namespace RentnRoll.Application.Contracts.Lockers.AssignGames;

public record AssignGamesRequest(
    ICollection<GameAssignment> GameAssignments);