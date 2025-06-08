using RentnRoll.Domain.Entities.Tags;

namespace RentnRoll.Application.Contracts.Tags.CreateTag;

public record CreateTagRequest(
    string Name,
    string Description
)
{
    public Tag ToTag(Guid businessId)
    {
        return new Tag
        {
            Name = Name,
            Description = Description,
            BusinessId = businessId,
        };
    }
};