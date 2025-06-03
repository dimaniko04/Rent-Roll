using RentnRoll.Domain.Entities.Games;

namespace RentnRoll.Application.Contracts.Games.Response;

public record GameResponse(
    Guid Id,
    string Name,
    string Description,
    string? ThumbnailUrl,
    DateTime PublishedAt,
    bool IsVerified
)
{
    public static GameResponse FromGame(Game game)
    {
        return new GameResponse(
            game.Id,
            game.Name,
            game.Description,
            game.ThumbnailUrl,
            game.PublishedAt,
            game.IsVerified
        );
    }
};