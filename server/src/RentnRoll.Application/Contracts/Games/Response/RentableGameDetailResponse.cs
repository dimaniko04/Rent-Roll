using RentnRoll.Domain.Entities.PricingPolicies.Enums;

namespace RentnRoll.Application.Contracts.Games.Response;

public record RentableGameDetailsResponse(
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
    Guid PricingPolicyItemId,
    int Price,
    int UnitCount,
    TimeUnit TimeUnit,
    Guid LocationId,
    string LocationName,
    string LocationType,
    string LocationAddress
);