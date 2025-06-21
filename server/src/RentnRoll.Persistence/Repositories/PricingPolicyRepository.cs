using Microsoft.EntityFrameworkCore;

using RentnRoll.Application.Common.Interfaces.Repositories;
using RentnRoll.Domain.Entities.Lockers;
using RentnRoll.Domain.Entities.PricingPolicies;
using RentnRoll.Domain.Entities.PricingPolicies.Enums;
using RentnRoll.Domain.Entities.Stores;
using RentnRoll.Persistence.Context;

namespace RentnRoll.Persistence.Repositories;

public class PricingPolicyRepository
    : BaseRepository<PricingPolicy>, IPricingPolicyRepository
{
    public PricingPolicyRepository(RentnRollDbContext context)
        : base(context)
    {
    }

    public async Task<PricingPolicyItem?>
        GetCellPolicyAsync(
            Guid cellId,
            TimeUnit unit)
    {
        var joined = await _dbSet
            .AsNoTracking()
            .Where(pp => pp.TimeUnit == unit)
            .Include(pp => pp.Items)
            .SelectMany(pp => pp.Items)
            .Join(
                _context.Set<Cell>(),
                item => item.GameId,
                cell => cell.BusinessGameId,
                (item, cell) => new
                {
                    Item = item,
                    CellId = cell.Id
                })
            .FirstOrDefaultAsync(x => x.CellId == cellId);

        return joined?.Item;
    }

    public async Task<PricingPolicyItem?>
        GetAssetPolicyAsync(
            Guid storeAssetId,
            TimeUnit unit)
    {
        var joined = await _dbSet
            .AsNoTracking()
            .Where(pp => pp.TimeUnit == unit)
            .Include(pp => pp.Items)
            .SelectMany(pp => pp.Items)
            .Join(
                _context.Set<StoreAsset>(),
                item => item.GameId,
                asset => asset.BusinessGameId,
                (item, asset) => new
                {
                    Item = item,
                    AssetId = asset.Id
                })
            .FirstOrDefaultAsync(x => x.AssetId == storeAssetId);

        return joined?.Item;
    }
}