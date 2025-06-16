namespace RentnRoll.Application.Contracts.Games.Response;

public record RentableGameResponse(
    Guid BusinessGameId,
    Guid LocationId,
    string LocationType,
    string Name,
    string Description,
    string? ThumbnailUrl,
    DateTime PublishedAt,
    bool IsVerified,
    int Price);