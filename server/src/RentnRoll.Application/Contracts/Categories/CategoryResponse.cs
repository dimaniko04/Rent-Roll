using RentnRoll.Domain.Entities.Categories;

namespace RentnRoll.Application.Contracts.Categories;

public record CategoryResponse(Guid Id, string Name)
{
    public static CategoryResponse FromCategory(Category category)
    {
        return new CategoryResponse(category.Id, category.Name);
    }
}