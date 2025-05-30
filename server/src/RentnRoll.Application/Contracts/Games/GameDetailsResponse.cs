using RentnRoll.Application.Contracts.Categories;
using RentnRoll.Application.Contracts.Genres;
using RentnRoll.Application.Contracts.Mechanics;
using RentnRoll.Domain.Entities.Games;

namespace RentnRoll.Application.Contracts.Games;

public record GameDetailsResponse(
    Guid Id,
    string Name,
    string Description,
    string ThumbnailUrl,
    DateTime PublishedAt,
    int MinPlayers,
    int MaxPlayers,
    int Age,
    int? AveragePlayTime,
    int? ComplexityScore,
    bool IsVerified,
    string? VerifiedByUserId,
    IEnumerable<GenreResponse> Genres,
    IEnumerable<CategoryResponse> Categories,
    IEnumerable<MechanicResponse> Mechanics,
    IEnumerable<string> Images
)
{
    public static GameDetailsResponse FromGame(Game game)
    {
        return new GameDetailsResponse(
            game.Id,
            game.Name,
            game.Description,
            game.ThumbnailUrl,
            game.PublishedAt,
            game.MinPlayers,
            game.MaxPlayers,
            game.Age,
            game.AveragePlayTime,
            game.ComplexityScore,
            game.IsVerified,
            game.VerifiedByUserId,
            game.Genres.Select(g =>
                new GenreResponse(g.Id, g.Name)),
            game.Categories.Select(c =>
                new CategoryResponse(c.Id, c.Name)),
            game.Mechanics.Select(m =>
                new MechanicResponse(m.Id, m.Name)),
            game.Images.Select(i => i.Url)
        );
    }
};