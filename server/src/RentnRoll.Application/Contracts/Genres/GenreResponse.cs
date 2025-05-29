using RentnRoll.Domain.Entities.Genres;

namespace RentnRoll.Application.Contracts.Genres;

public record GenreResponse(Guid Id, string Name)
{
    public static GenreResponse FromGenre(Genre Genre)
    {
        return new GenreResponse(Genre.Id, Genre.Name);
    }
}