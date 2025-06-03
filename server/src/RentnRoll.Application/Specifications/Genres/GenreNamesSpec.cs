using RentnRoll.Application.Specifications.Common;
using RentnRoll.Domain.Entities.Genres;

namespace RentnRoll.Application.Specifications.Genres;

public sealed class GenreNamesSpec : Specification<Genre>
{
    public GenreNamesSpec(ICollection<string> names)
    {
        AddCriteria(g => names.Contains(g.Name));
    }
}