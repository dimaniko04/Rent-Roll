using RentnRoll.Application.Contracts.BusinessGames.Response;
using RentnRoll.Application.Contracts.Common;
using RentnRoll.Application.Specifications.Common;
using RentnRoll.Domain.Entities.BusinessGames;

namespace RentnRoll.Application.Common.Interfaces.Repositories;

public interface IBusinessGameRepository : IBaseRepository<BusinessGame>
{
    public Task<PaginatedResponse<BusinessGameResponse>> GetPaginatedAsync(
        ISpecification<BusinessGame> specification);
    public Task DeleteRangeAsync(
        IEnumerable<BusinessGame> businessGames);
}