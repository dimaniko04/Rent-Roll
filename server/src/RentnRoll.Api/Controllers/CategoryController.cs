using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using RentnRoll.Application.Contracts.Categories;
using RentnRoll.Application.Services.Categories;
using RentnRoll.Domain.Constants;

namespace RentnRoll.Api.Controllers;

[ApiController]
[Route("api/categories")]
[Authorize]
public class CategoryController : ApiController
{
    private readonly ICategoryService _categoryService;

    public CategoryController(ICategoryService categoryService)
    {
        _categoryService = categoryService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllCategories(
        [FromQuery] GetAllCategoriesRequest request)
    {
        var result = await _categoryService
            .GetAllCategoriesAsync(request);
        return Ok(result);
    }

    [HttpGet("{categoryId:guid}")]
    public async Task<IActionResult> GetCategoryById(
        Guid categoryId)
    {
        var result = await _categoryService
            .GetCategoryByIdAsync(categoryId);
        return result.Match(Ok, Problem);
    }

    [HttpPost]
    [Authorize(Roles = Roles.Admin)]
    public async Task<IActionResult> CreateCategory(
        CreateCategoryRequest request)
    {
        var result = await _categoryService
            .CreateCategoryAsync(request);
        return result.Match(Ok, Problem);
    }

    [HttpPut("{categoryId:guid}")]
    [Authorize(Roles = Roles.Admin)]
    public async Task<IActionResult> UpdateCategory(
        Guid categoryId,
        UpdateCategoryRequest request)
    {
        var result = await _categoryService
            .UpdateCategoryAsync(categoryId, request);
        return result.Match(Ok, Problem);
    }

    [HttpDelete("{categoryId:guid}")]
    [Authorize(Roles = Roles.Admin)]
    public async Task<IActionResult> DeleteCategory(
        Guid categoryId)
    {
        var result = await _categoryService
            .DeleteCategoryAsync(categoryId);
        return result.Match(Ok, Problem);
    }
}