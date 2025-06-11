namespace RentnRoll.Application.Contracts.Lockers.Response;

public record CellResponse(
    Guid Id,
    string Status,
    string? BusinessName,
    string? BusinessGameName,
    string? IotDeviceId
)
{
    public static CellResponse FromCell(Domain.Entities.Lockers.Cell cell)
    {
        return new CellResponse(
            cell.Id,
            Enum.GetName(cell.Status) ?? "Unknown",
            cell.Business != null ?
                cell.Business.Name :
                null,
            cell.BusinessGame != null ?
                cell.BusinessGame.Game.Name :
                null,
            cell.IotDeviceId
        );
    }
}