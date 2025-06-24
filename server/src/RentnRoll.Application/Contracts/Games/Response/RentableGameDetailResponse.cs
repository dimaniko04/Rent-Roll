using RentnRoll.Domain.Entities.PricingPolicies.Enums;

namespace RentnRoll.Application.Contracts.Games.Response;

public record RentableGamePrice(
    int UnitCount,
    string TimeUnit,
    int Price
);

public record RentableGameDetailsResponse(
    Guid id,
    Guid BusinessGameId,
    string Name,
    string Description,
    string? ThumbnailUrl,
    DateTime PublishedAt,
    int MinPlayers,
    int MaxPlayers,
    int Age,
    int? AveragePlayTime,
    int? ComplexityScore,
    bool IsVerified,
    IEnumerable<string> Genres,
    IEnumerable<string> Categories,
    IEnumerable<string> Mechanics,
    IEnumerable<string> Images,
    IEnumerable<RentableGamePrice> Prices,
    string LocationName,
    string LocationType,
    string LocationAddress
);