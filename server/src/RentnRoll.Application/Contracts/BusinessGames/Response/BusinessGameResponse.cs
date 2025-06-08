using RentnRoll.Application.Contracts.Tags.Response;
using RentnRoll.Domain.Entities.BusinessGames;

namespace RentnRoll.Application.Contracts.BusinessGames.Response;

public record BusinessGameResponse(
    Guid Id,
    string GameName,
    string? GameThumbnailUrl,
    bool IsVerified,
    int Quantity,
    int BasePrice,
    ICollection<TagResponse> Tags
)
{
    public static BusinessGameResponse FromBusinessGame(
        BusinessGame game)
    {
        return new BusinessGameResponse(
            game.Id,
            game.Game.Name,
            game.Game.ThumbnailUrl,
            game.Game.IsVerified,
            game.Quantity,
            game.BasePrice,
            game.Tags
                .Select(TagResponse.FromTag)
                .ToList()
        );
    }
}