using RentnRoll.Application.Contracts.Businesses.Response;
using RentnRoll.Application.Contracts.Common;
using RentnRoll.Application.Specifications.Common;
using RentnRoll.Domain.Entities.Businesses;

namespace RentnRoll.Application.Common.Interfaces.Repositories;

public interface IBusinessRepository : IBaseRepository<Business>
{
    Task<PaginatedResponse<BusinessWithOwnerResponse>>
        GetPaginatedWithOwnerAsync(
            ISpecification<Business> specification);
    Task<BusinessWithOwnerResponse?> GetSingleWithOwnerAsync(
        ISpecification<Business> specification);
}