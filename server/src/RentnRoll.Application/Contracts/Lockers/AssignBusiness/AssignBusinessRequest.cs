namespace RentnRoll.Application.Contracts.Lockers.AssignBusiness;

public record AssignBusinessRequest(
    Guid BusinessId,
    int CellCount);