using RentnRoll.Application.Common.Interfaces.Repositories;
using RentnRoll.Persistence.Context;
using RentnRoll.Domain.Entities.Businesses;
using RentnRoll.Application.Contracts.Common;
using RentnRoll.Application.Contracts.Businesses.Response;
using RentnRoll.Application.Specifications.Common;
using RentnRoll.Persistence.Specifications;
using RentnRoll.Persistence.Extensions;
using Microsoft.EntityFrameworkCore;

namespace RentnRoll.Persistence.Repositories;

public class BusinessRepository : BaseRepository<Business>, IBusinessRepository
{
    public BusinessRepository(RentnRollDbContext context)
        : base(context)
    {
    }

    public Task<Business?> GetByOwnerIdAsync(string ownerId)
    {
        return _dbSet
            .AsNoTracking()
            .FirstOrDefaultAsync(b => b.OwnerId == ownerId);
    }

    public async Task<PaginatedResponse<BusinessWithOwnerResponse>>
        GetPaginatedWithOwnerAsync(
            ISpecification<Business> specification)
    {
        var query = SpecificationEvaluator.GetQuery(
            _dbSet,
            specification
        );

        var businesses = await query.Join(
            _context.Users,
            business => business.OwnerId,
            user => user.Id,
            (business, user) => new
            {
                Business = business,
                User = user.ToUserResponse()
            }
        ).Select(o => BusinessWithOwnerResponse.
            FromBusiness(o.Business, o.User))
        .ToPaginatedResponse(
            specification.PageNumber,
            specification.PageSize
        );

        return businesses;
    }

    public async Task<BusinessWithOwnerResponse?>
        GetSingleWithOwnerAsync(
            ISpecification<Business> specification)
    {
        var query = SpecificationEvaluator.GetQuery(
            _dbSet,
            specification
        );

        var businesses = await query.Join(
            _context.Users,
            business => business.OwnerId,
            user => user.Id,
            (business, user) => new
            {
                Business = business,
                User = user.ToUserResponse()
            }
        ).Select(o => BusinessWithOwnerResponse.
            FromBusiness(o.Business, o.User))
        .FirstOrDefaultAsync();

        return businesses;
    }
}