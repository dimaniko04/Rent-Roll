namespace RentnRoll.Application.Contracts.BusinessGames.DeleteBusinessGames;

public record DeleteBusinessGamesRequest(
    ICollection<Guid> Ids);
