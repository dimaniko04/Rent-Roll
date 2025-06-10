namespace RentnRoll.Application.Contracts.Stores.UpdateStore;

public record UpdateStoreAssetRequest(
    int Quantity,
    Guid BusinessGameId);