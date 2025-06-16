using RentnRoll.Domain.Entities.Lockers;

namespace RentnRoll.Domain.Entities.Rentals;

public class LockerRental
{
    public Guid RentalId { get; set; }
    public Rental Rental { get; set; } = null!;

    public Guid CellId { get; set; }
    public Cell? Cell { get; set; }
}