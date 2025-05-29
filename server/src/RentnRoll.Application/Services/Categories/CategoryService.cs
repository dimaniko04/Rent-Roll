using RentnRoll.Application.Common.Interfaces.Repositories;
using RentnRoll.Application.Contracts.Categories;
using RentnRoll.Application.Specifications.Categories;
using RentnRoll.Domain.Common;
using RentnRoll.Application.Common.AppErrors;
using RentnRoll.Application.Services.Validation;
using RentnRoll.Application.Common.Interfaces.UnitOfWork;
using Microsoft.Extensions.Logging;

namespace RentnRoll.Application.Services.Categories;

public class CategoryService : ICategoryService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICategoryRepository _categoryRepository;
    private readonly IValidationService _validationService;

    public CategoryService(
        IUnitOfWork unitOfWork,
        ILogger<CategoryService> logger,
        IValidationService validationService)
    {
        _unitOfWork = unitOfWork;
        _validationService = validationService;
        _categoryRepository = unitOfWork
            .GetRepository<ICategoryRepository>();
    }

    public async Task<IEnumerable<CategoryResponse>> GetAllCategoriesAsync(
        GetAllCategoriesRequest request)
    {
        var specification = new GetAllCategoriesRequestSpec(request);

        var categories = await _categoryRepository
            .GetAllAsync(specification);
        var categoryResponses = categories
            .Select(CategoryResponse.FromCategory);

        return categoryResponses;
    }

    public async Task<Result<CategoryResponse>> GetCategoryByIdAsync(
        Guid categoryId)
    {
        var category = await _categoryRepository.GetByIdAsync(categoryId);

        if (category == null)
            return Errors.Categories.NotFound;

        var categoryResponse = CategoryResponse
            .FromCategory(category);

        return categoryResponse;
    }

    public async Task<Result<CategoryResponse>> CreateCategoryAsync(
        CreateCategoryRequest request)
    {
        var validationResult = await _validationService
            .ValidateAsync(request);
        if (validationResult.IsError)
            return validationResult.Errors;

        var existingCategory = await _categoryRepository
            .GetByNameAsync(request.Name);

        if (existingCategory != null)
            return Errors.Categories.AlreadyExists(request.Name);

        var category = request.ToCategory();
        await _categoryRepository.CreateAsync(category);
        await _unitOfWork.SaveChangesAsync();

        var categoryResponse = CategoryResponse
            .FromCategory(category);

        return categoryResponse;
    }

    public async Task<Result<CategoryResponse>> UpdateCategoryAsync(
        Guid id,
        UpdateCategoryRequest request)
    {
        var validationResult = await _validationService
                    .ValidateAsync(request);
        if (validationResult.IsError)
            return validationResult.Errors;

        var category = await _categoryRepository
            .GetByIdAsync(id);

        if (category == null)
            return Errors.Categories.NotFound;

        category.Name = request.Name;
        _categoryRepository.Update(category);
        await _unitOfWork.SaveChangesAsync();

        var categoryResponse = CategoryResponse
            .FromCategory(category);

        return categoryResponse;
    }

    public async Task<Result> DeleteCategoryAsync(Guid categoryId)
    {
        var category = await _categoryRepository.GetByIdAsync(categoryId);
        if (category == null)
            return Result.Failure([Errors.Categories.NotFound]);

        _categoryRepository.Delete(category);
        await _unitOfWork.SaveChangesAsync();

        return Result.Success();
    }
}