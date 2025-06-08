using RentnRoll.Domain.Entities.Tags;

namespace RentnRoll.Application.Contracts.Tags.Response;

public record TagResponse(
    Guid Id,
    string Name
)
{
    public static TagResponse FromTag(Tag tag)
    {
        return new TagResponse(
            tag.Id,
            tag.Name
        );
    }
}