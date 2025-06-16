namespace RentnRoll.Application.Contracts.Rentals.Response;

public record RentalResponse(
    Guid Id,
    string UserId,
    string UserName,
    string Status,
    DateTime StartDate,
    DateTime EndDate,
    int TotalPrice,
    string Address,
    string GameName,
    string LocationName);