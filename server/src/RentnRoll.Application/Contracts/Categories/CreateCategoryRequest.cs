using RentnRoll.Domain.Entities.Categories;

namespace RentnRoll.Application.Contracts.Categories;

public record CreateCategoryRequest(string Name)
{
    public Category ToCategory()
    {
        return new Category
        {
            Id = Guid.NewGuid(),
            Name = Name
        };
    }
};