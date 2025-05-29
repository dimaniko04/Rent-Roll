using RentnRoll.Application.Contracts.Categories;
using RentnRoll.Domain.Common;
using RentnRoll.Domain.Entities.Categories;

namespace RentnRoll.Application.Services.Categories;

public interface ICategoryService
{
    Task<Result<CategoryResponse>> CreateCategoryAsync(
        CreateCategoryRequest request);

    Task<Result<CategoryResponse>> UpdateCategoryAsync(
        Guid categoryId,
        UpdateCategoryRequest request);

    Task<Result<CategoryResponse>> GetCategoryByIdAsync(
        Guid categoryId);

    Task<IEnumerable<CategoryResponse>> GetAllCategoriesAsync(
        GetAllCategoriesRequest request);

    Task<Result> DeleteCategoryAsync(Guid categoryId);
}