using RentnRoll.Domain.Entities.Tags;

namespace RentnRoll.Application.Contracts.Tags.UpdateTag;

public record UpdateTagRequest(
    string Name,
    string Description
)
{
    public Tag UpdateTag(Tag tag)
    {
        tag.Name = Name;
        tag.Description = Description;

        return tag;
    }
}