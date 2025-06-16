using RentnRoll.Domain.Entities.Stores;

namespace RentnRoll.Domain.Entities.Rentals;

public class StoreRental
{
    public Guid RentalId { get; set; }
    public Rental Rental { get; set; } = null!;

    public Guid StoreId { get; set; }
    public Store? Store { get; set; }
}