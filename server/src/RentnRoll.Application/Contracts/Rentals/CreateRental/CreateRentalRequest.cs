namespace RentnRoll.Application.Contracts.Rentals.CreateRental;

public class CreateRentalRequest
{
    public Guid StoreAssetId { get; set; }
    public Guid CellId { get; set; }
    public int Term { get; set; }
}