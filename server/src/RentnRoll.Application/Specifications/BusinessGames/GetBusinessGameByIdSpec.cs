using RentnRoll.Application.Specifications.Common;
using RentnRoll.Domain.Entities.BusinessGames;

namespace RentnRoll.Application.Specifications.BusinessGames;

public sealed class GetBusinessGameByIdSpec : Specification<BusinessGame>
{
    public GetBusinessGameByIdSpec(
        Guid businessId,
        Guid businessGameId)
    {
        AddInclude(bg => bg.Game);
        AddInclude(bg => bg.Tags);

        AddCriteria(bg => bg.BusinessId == businessId);
        AddCriteria(bg => bg.Id == businessGameId);
        AddInclude(bg => bg.Game);
    }
}