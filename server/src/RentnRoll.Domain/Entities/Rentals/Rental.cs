using RentnRoll.Domain.Common;
using RentnRoll.Domain.Entities.Rentals.Enums;

namespace RentnRoll.Domain.Entities.Rentals;

public class Rental : Entity
{
    public string UserId { get; set; } = null!;
    public RentalStatus Status { get; set; } = RentalStatus.Expectation;

    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }

    public int TotalPrice { get; set; }
    public string Address { get; set; } = null!;
    public string GameName { get; set; } = null!;
    public string LocationName { get; set; } = null!;
    public string? IotDeviceId { get; set; }
}