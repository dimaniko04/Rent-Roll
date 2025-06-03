using RentnRoll.Domain.Entities.Games;

namespace RentnRoll.Application.Contracts.Games.CreateGame;

public record CreateGameRequest(
    string Name,
    string Description,
    DateTime PublishedAt,
    int MinPlayers,
    int MaxPlayers,
    int Age
)
{
    public Game ToGame(bool isVerified, string createdBy)
    {
        return new Game
        {
            Name = Name,
            Description = Description,
            PublishedAt = PublishedAt,
            MinPlayers = MinPlayers,
            MaxPlayers = MaxPlayers,
            Age = Age,
            IsVerified = isVerified,
            VerifiedByUserId = isVerified ? createdBy : null,
            CreatedByUserId = createdBy,
        };
    }
};