using RentnRoll.Application.Contracts.Common;
using RentnRoll.Domain.Entities.PricingPolicies.Enums;
using RentnRoll.Domain.Enums;

namespace RentnRoll.Application.Contracts.Games.GetAllGames;

public record GetAllRentableGamesRequest(
    int? MinPlayers,
    int? MaxPlayers,
    int? MinPlayTime,
    int? MaxPlayTime,
    string? City,
    TimeUnit? TimeUnit,
    int MinPrice,
    int MaxPrice,
    LocationType? LocationType,
    IEnumerable<string>? Genres,
    IEnumerable<string>? Categories,
    IEnumerable<string>? Mechanics,
    bool? IsVerified,
    int Age = 0
) : GetAllGamesRequest(
    MinPlayers,
    MaxPlayers,
    MinPlayTime,
    MaxPlayTime,
    Genres,
    Categories,
    Mechanics,
    IsVerified,
    Age
);