using Microsoft.EntityFrameworkCore;

using RentnRoll.Application.Common.Interfaces.Repositories;
using RentnRoll.Application.Contracts.BusinessGames.Response;
using RentnRoll.Application.Contracts.Common;
using RentnRoll.Application.Contracts.Stores.Response;
using RentnRoll.Application.Contracts.Tags.Response;
using RentnRoll.Application.Specifications.Common;
using RentnRoll.Domain.Entities.Stores;
using RentnRoll.Persistence.Context;
using RentnRoll.Persistence.Extensions;
using RentnRoll.Persistence.Specifications;

namespace RentnRoll.Persistence.Repositories;

public class StoreRepository : BaseRepository<Store>, IStoreRepository
{
    public StoreRepository(RentnRollDbContext context)
        : base(context)
    {
    }

    public async Task<PaginatedResponse<StoreAssetResponse>>
        GetPaginatedStoreAssetsAsync(
            Specification<StoreAsset> specification,
            bool trackChanges = false)
    {
        var query = SpecificationEvaluator.GetQuery(
            _context.Set<StoreAsset>(),
            specification);

        if (!trackChanges)
        {
            query = query.AsNoTracking();
        }

        var assets = await query
            .Select(a => new StoreAssetResponse(
                a.Quantity,
                new BusinessGameResponse(
                    a.BusinessGame.Id,
                    a.BusinessGame.Game.Name,
                    a.BusinessGame.Game.ThumbnailUrl,
                    a.BusinessGame.Game.IsVerified,
                    a.BusinessGame.Quantity,
                    a.BusinessGame.BasePrice,
                    a.BusinessGame.Tags.Select(t => new TagResponse(
                        t.Id,
                        t.Name
                    )).ToList()
                )))
            .ToPaginatedResponse(
                specification.PageNumber,
                specification.PageSize);

        return assets;
    }
}