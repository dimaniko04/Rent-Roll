namespace RentnRoll.Application.Contracts.Lockers.ConfigureCells;

public record ConfigureCellsRequest(
    string DeviceId,
    ICollection<int> Pins);
