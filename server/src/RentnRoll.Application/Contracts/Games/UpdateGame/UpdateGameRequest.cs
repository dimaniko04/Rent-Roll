using RentnRoll.Application.Contracts.Games.CreateGame;
using RentnRoll.Domain.Common;
using RentnRoll.Domain.Entities.Categories;
using RentnRoll.Domain.Entities.Games;
using RentnRoll.Domain.Entities.Genres;
using RentnRoll.Domain.Entities.Mechanics;

namespace RentnRoll.Application.Contracts.Games.UpdateGame;

public record UpdateGameRequest(
    string Name,
    string Description,
    DateTime PublishedAt,
    int MinPlayers,
    int MaxPlayers,
    int Age,
    int? AveragePlayTime,
    int? ComplexityScore,
    ICollection<string>? Genres,
    ICollection<string>? Categories,
    ICollection<string>? Mechanics
) : CreateGameRequest(
    Name,
    Description,
    PublishedAt,
    MinPlayers,
    MaxPlayers,
    Age
)
{
    public Result<Game> UpdateGame(
        Game game,
        List<Genre> genres,
        List<Category> categories,
        List<Mechanic> mechanics)
    {
        game.Name = Name;
        game.Description = Description;
        game.PublishedAt = PublishedAt;
        game.MinPlayers = MinPlayers;
        game.MaxPlayers = MaxPlayers;
        game.Age = Age;
        game.AveragePlayTime = AveragePlayTime;
        game.ComplexityScore = ComplexityScore;

        if (genres.Any())
        {
            game.Genres = genres;
        }
        if (categories.Any())
        {
            game.Categories = categories;
        }
        if (mechanics.Any())
        {
            game.Mechanics = mechanics;
        }

        return game;
    }
};