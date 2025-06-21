using RentnRoll.Application.Contracts.Common;
using RentnRoll.Application.Contracts.Stores.Response;
using RentnRoll.Application.Specifications.Common;
using RentnRoll.Domain.Entities.Stores;

namespace RentnRoll.Application.Common.Interfaces.Repositories;

public interface IStoreRepository : IBaseRepository<Store>
{
    Task<PaginatedResponse<StoreAssetResponse>>
        GetPaginatedStoreAssetsAsync(
            Specification<StoreAsset> specification,
            bool trackChanges = false);
    Task<StoreAsset?> GetStoreAssetByIdAsync(
        Guid storeAssetId);
}