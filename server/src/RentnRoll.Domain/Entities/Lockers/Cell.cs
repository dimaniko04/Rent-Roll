using RentnRoll.Domain.Common;
using RentnRoll.Domain.Entities.Businesses;
using RentnRoll.Domain.Entities.BusinessGames;
using RentnRoll.Domain.Entities.Lockers.Enums;

namespace RentnRoll.Domain.Entities.Lockers;

public class Cell : Entity
{
    public string? IotDeviceId { get; set; }
    public CellStatus Status { get; set; } = CellStatus.Empty;

    public Guid? BusinessId { get; set; }
    public Business? Business { get; set; }

    public Guid? BusinessGameId { get; set; }
    public BusinessGame? BusinessGame { get; set; }

    public Guid LockerId { get; set; }
    public Locker Locker { get; set; } = null!;
}