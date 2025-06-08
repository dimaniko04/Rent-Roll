using RentnRoll.Domain.Entities.Tags;

namespace RentnRoll.Application.Contracts.Tags.Response;

public record TagDetailResponse(
    Guid Id,
    string Name,
    string Description
)
{
    public static TagDetailResponse FromTag(Tag tag)
    {
        return new TagDetailResponse(
            tag.Id,
            tag.Name,
            tag.Description
        );
    }
}