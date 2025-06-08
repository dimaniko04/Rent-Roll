using RentnRoll.Application.Contracts.Common;
using RentnRoll.Domain.Entities.Tags;

namespace RentnRoll.Application.Contracts.BusinessGames.GetAllBusinessGames;

public record GetAllBusinessGamesRequest(
    Tag? Tag,
    bool? isVerified,
    int? MinPlayers,
    int? MaxPlayers,
    int? MinPlayTime,
    int? MaxPlayTime,
    IEnumerable<string>? Genres,
    IEnumerable<string>? Categories,
    IEnumerable<string>? Mechanics,
    int Age = 0,
    List<Guid>? ExcludeIds = null
) : QueryParams;