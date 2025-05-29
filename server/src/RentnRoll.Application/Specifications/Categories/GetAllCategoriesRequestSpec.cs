using RentnRoll.Application.Contracts.Categories;
using RentnRoll.Application.Specifications.Common;
using RentnRoll.Domain.Entities.Categories;

namespace RentnRoll.Application.Specifications.Categories;

public sealed class GetAllCategoriesRequestSpec : Specification<Category>
{
    public GetAllCategoriesRequestSpec(GetAllCategoriesRequest request)
    {
        if (!string.IsNullOrEmpty(request.Search))
        {
            AddCriteria(c => c.Name.Contains(request.Search));
        }

        ApplySorting(request.SortBy);
    }
}