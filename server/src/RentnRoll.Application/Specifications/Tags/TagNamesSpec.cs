using RentnRoll.Application.Specifications.Common;
using RentnRoll.Domain.Entities.Tags;

namespace RentnRoll.Application.Specifications.Tags;

public sealed class TagNamesSpec : Specification<Tag>
{
    public TagNamesSpec(
        Guid businessId,
        ICollection<string> tagNames)
    {
        AddCriteria(tag => tag.BusinessId == businessId);
        AddCriteria(tag => tagNames.Contains(tag.Name));
    }
}