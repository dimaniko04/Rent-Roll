using RentnRoll.Application.Specifications.Common;
using RentnRoll.Domain.Entities.Tags;

namespace RentnRoll.Application.Specifications.Tags;

public sealed class TagIdSpec : Specification<Tag>
{
    public TagIdSpec(Guid businessId, Guid tagId)
    {
        AddCriteria(tag => tag.BusinessId == businessId);
        AddCriteria(tag => tag.Id == tagId);
    }
}