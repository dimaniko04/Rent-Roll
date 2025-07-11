using RentnRoll.Application.Contracts.Common;

namespace RentnRoll.Application.Contracts.Games.GetAllGames;

public record GetAllGamesRequest(
    int? MinPlayers,
    int? MaxPlayers,
    int? MinPlayTime,
    int? MaxPlayTime,
    IEnumerable<string>? Genres,
    IEnumerable<string>? Categories,
    IEnumerable<string>? Mechanics,
    bool? IsVerified,
    int Age = 0
) : QueryParams;