using RentnRoll.Application.Contracts.BusinessGames.Response;
using RentnRoll.Domain.Entities.Stores;

namespace RentnRoll.Application.Contracts.Stores.Response;

public record StoreAssetResponse(
    int Quantity,
    BusinessGameResponse Game
)
{
    public static StoreAssetResponse FromStoreAsset(StoreAsset asset)
    {
        return new StoreAssetResponse(
            asset.Quantity,
            BusinessGameResponse
                .FromBusinessGame(asset.BusinessGame)
        );
    }
}