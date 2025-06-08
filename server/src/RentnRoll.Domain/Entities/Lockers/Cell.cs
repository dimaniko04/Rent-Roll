using RentnRoll.Domain.Common;
using RentnRoll.Domain.Entities.Businesses;
using RentnRoll.Domain.Entities.Lockers.Enums;

namespace RentnRoll.Domain.Entities.Lockers;

public class Cell : Entity
{
    public CellStatus Status { get; set; }
    public string? IotDeviceUrl { get; set; }

    public Guid? BusinessId { get; set; }
    public Business? Business { get; set; }

    public Guid LockerId { get; set; }
    public Locker Locker { get; set; } = null!;
}