using RentnRoll.Application.Contracts.Tags.GetAllTags;
using RentnRoll.Application.Specifications.Common;
using RentnRoll.Domain.Entities.Tags;

namespace RentnRoll.Application.Specifications.Tags;

public class GetAllTagsRequestSpec : Specification<Tag>
{
    public GetAllTagsRequestSpec(GetAllTagsRequest request)
    {
        if (!string.IsNullOrEmpty(request.Search))
        {
            AddCriteria(tag => tag.Name.Contains(request.Search));
        }

        ApplyOrderBy(x => x.Name);
    }

    public GetAllTagsRequestSpec(Guid businessId, GetAllTagsRequest request)
    {
        AddCriteria(tag => tag.BusinessId == businessId);

        if (!string.IsNullOrEmpty(request.Search))
        {
            AddCriteria(tag => tag.Name.Contains(request.Search));
        }

        ApplyOrderBy(x => x.Name);
    }
}