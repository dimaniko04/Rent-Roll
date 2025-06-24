namespace RentnRoll.Application.Contracts.Rentals.Response;

public record UserRentalResponse(
    Guid Id,
    string Status,
    DateTime StartDate,
    DateTime EndDate,
    int TotalPrice,
    string Address,
    string GameName,
    string? GameThumbnail,
    string LocationName,
    string? IotDeviceId);