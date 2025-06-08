using RentnRoll.Application.Specifications.Common;
using RentnRoll.Domain.Entities.BusinessGames;

namespace RentnRoll.Application.Specifications.BusinessGames;

public sealed class GetBusinessGamesByIdsSpec
    : Specification<BusinessGame>
{
    public GetBusinessGamesByIdsSpec(
        Guid businessId,
        ICollection<Guid> businessGameIds)
    {
        AddInclude(bg => bg.Game);
        AddInclude(bg => bg.Tags);

        AddCriteria(businessGame =>
            businessGame.BusinessId == businessId);
        AddCriteria(businessGame =>
            businessGameIds.Contains(businessGame.Id));
    }
}