using RentnRoll.Application.Contracts.Stores.CreateStore;
using RentnRoll.Application.Contracts.Stores.GetAllStores;
using RentnRoll.Application.Contracts.Stores.Response;
using RentnRoll.Application.Contracts.Stores.UpdateStore;
using RentnRoll.Domain.Common;

namespace RentnRoll.Application.Services.Stores;

public interface IStoreService
{
    Task<ICollection<StoreResponse>> GetAllStoresAsync(
        Guid businessId,
        GetAllStoresRequest request);
    Task<Result<StoreDetailsResponse>> GetStoreByIdAsync(
        Guid businessId,
        Guid storeId);
    Task<Result<StoreResponse>> CreateStoreAsync(
        Guid businessId,
        CreateStoreRequest request);
    Task<Result<StoreDetailsResponse>> UpdateStoreAsync(
        Guid businessId,
        Guid storeId,
        UpdateStoreRequest request);
    Task<Result> DeleteStoreAsync(
        Guid businessId,
        Guid storeId);
}