using RentnRoll.Domain.Entities.Genres;

namespace RentnRoll.Application.Contracts.Genres;

public record UpdateGenreRequest(string Name)
{
    public Genre ToGenre()
    {
        return new Genre
        {
            Id = Guid.NewGuid(),
            Name = Name
        };
    }
};