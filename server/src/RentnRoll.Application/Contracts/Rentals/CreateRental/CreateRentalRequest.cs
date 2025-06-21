namespace RentnRoll.Application.Contracts.Rentals.CreateRental;

public class CreateRentalRequest
{
    public Guid Id { get; set; }
    public int Term { get; set; }
    public string Unit { get; set; } = null!;
    public string Type { get; set; } = null!;
}