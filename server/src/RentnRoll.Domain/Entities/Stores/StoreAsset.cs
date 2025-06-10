using RentnRoll.Domain.Entities.BusinessGames;

namespace RentnRoll.Domain.Entities.Stores;

public class StoreAsset
{
    public Guid StoreId { get; set; }
    public Store Store { get; set; } = null!;

    public Guid BusinessGameId { get; set; }
    public BusinessGame BusinessGame { get; set; } = null!;

    public int Quantity { get; set; }
}