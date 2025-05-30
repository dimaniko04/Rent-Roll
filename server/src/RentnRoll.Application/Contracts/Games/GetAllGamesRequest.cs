using RentnRoll.Application.Contracts.Common;

namespace RentnRoll.Application.Contracts.Games;

public record GetAllGamesRequest(
    int? MinPlayers,
    int? MaxPlayers,
    int? MinPlayTime,
    int? MaxPlayTime,
    IEnumerable<string>? Genres,
    IEnumerable<string>? Categories,
    IEnumerable<string>? Mechanics,
    int Age = 0,
    bool IsVerified = false
) : QueryParams;