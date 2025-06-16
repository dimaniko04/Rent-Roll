using RentnRoll.Domain.Common;
using RentnRoll.Domain.Entities.Businesses;
using RentnRoll.Domain.Entities.BusinessGames;
using RentnRoll.Domain.Entities.Games;
using RentnRoll.Domain.Entities.Rentals.Enums;
using RentnRoll.Domain.ValueObjects;

namespace RentnRoll.Domain.Entities.Rentals;

public class Rental : Entity
{
    public string UserId { get; set; } = null!;
    public RentalStatus Status { get; set; } = RentalStatus.Expectation;

    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public int TotalPrice { get; set; }

    public LockerRental? LockerRental { get; set; }
    public StoreRental? StoreRental { get; set; }

    public Business Business =>
        LockerRental?.Cell?.Business ??
        StoreRental?.StoreAsset?.Store?.Business ??
        throw new InvalidOperationException("Rental has not associated business");
    public Game Game =>
        LockerRental?.Cell?.BusinessGame?.Game ??
        StoreRental?.StoreAsset?.BusinessGame?.Game ??
        throw new InvalidOperationException("Rental has not associated game");
}