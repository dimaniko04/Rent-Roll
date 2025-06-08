

using Microsoft.EntityFrameworkCore;

using RentnRoll.Application.Common.Interfaces.Repositories;
using RentnRoll.Application.Contracts.BusinessGames.Response;
using RentnRoll.Application.Contracts.Common;
using RentnRoll.Application.Contracts.Tags.Response;
using RentnRoll.Application.Specifications.Common;
using RentnRoll.Domain.Entities.BusinessGames;
using RentnRoll.Persistence.Context;
using RentnRoll.Persistence.Extensions;
using RentnRoll.Persistence.Specifications;

namespace RentnRoll.Persistence.Repositories;

public class BusinessGameRepository
    : BaseRepository<BusinessGame>, IBusinessGameRepository
{
    public BusinessGameRepository(RentnRollDbContext context)
        : base(context)
    {
    }

    public Task<PaginatedResponse<BusinessGameResponse>>
        GetPaginatedAsync(
            ISpecification<BusinessGame> specification)
    {
        var query = SpecificationEvaluator
            .GetQuery(_dbSet.AsQueryable(), specification);

        return query
            .AsNoTracking()
            .Select(bg => new BusinessGameResponse(
                bg.Id,
                bg.Game.Name,
                bg.Game.ThumbnailUrl,
                bg.Game.IsVerified,
                bg.Quantity,
                bg.BasePrice,
                bg.Tags
                    .Select(t => new TagResponse(
                        t.Id,
                        t.Name))
                    .ToList()
            ))
            .ToPaginatedResponse(
                specification.PageNumber,
                specification.PageSize);
    }

    public Task DeleteRangeAsync(IEnumerable<BusinessGame> businessGames)
    {
        var ids = businessGames
            .Select(bg => bg.Id)
            .ToList();

        return _dbSet
            .Where(bg => ids.Contains(bg.Id))
            .ExecuteDeleteAsync();
    }
}