using RentnRoll.Application.Specifications.Common;
using RentnRoll.Domain.Entities.Tags;

namespace RentnRoll.Application.Specifications.Tags;

public sealed class TagNameSpec : Specification<Tag>
{
    public TagNameSpec(
        Guid businessId,
        string name)
    {
        AddCriteria(tag => tag.BusinessId == businessId);
        AddCriteria(tag => tag.Name == name);
    }
}