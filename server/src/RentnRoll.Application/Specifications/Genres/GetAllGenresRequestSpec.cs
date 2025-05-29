using RentnRoll.Application.Contracts.Genres;
using RentnRoll.Application.Specifications.Common;
using RentnRoll.Domain.Entities.Genres;

namespace RentnRoll.Application.Specifications.Genres;

public sealed class GetAllGenresRequestSpec : Specification<Genre>
{
    public GetAllGenresRequestSpec(GetAllGenresRequest request)
    {
        if (!string.IsNullOrEmpty(request.Search))
        {
            AddCriteria(c => c.Name.Contains(request.Search));
        }

        ApplySorting(request.SortBy);
    }
}