using RentnRoll.Application.Specifications.Common;
using RentnRoll.Domain.Entities.Categories;

namespace RentnRoll.Application.Specifications.Categories;

public sealed class CategoryNamesSpec : Specification<Category>
{
    public CategoryNamesSpec(ICollection<string> names)
    {
        AddCriteria(c => names.Contains(c.Name));
    }
}