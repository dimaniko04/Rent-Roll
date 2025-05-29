using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using RentnRoll.Application.Contracts.Genres;
using RentnRoll.Application.Services.Genres;

namespace RentnRoll.Api.Controllers;

[ApiController]
[Route("api/genres")]
[Authorize]
public class GenreController : ApiController
{
    private readonly IGenreService _genreService;

    public GenreController(IGenreService genreService)
    {
        _genreService = genreService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllGenres(
        [FromQuery] GetAllGenresRequest request)
    {
        var result = await _genreService
            .GetAllGenresAsync(request);
        return Ok(result);
    }

    [HttpGet("{genreId:guid}")]
    public async Task<IActionResult> GetGenreById(
        Guid genreId)
    {
        var result = await _genreService
            .GetGenreByIdAsync(genreId);
        return result.Match(Ok, Problem);
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> CreateGenre(
        CreateGenreRequest request)
    {
        var result = await _genreService
            .CreateGenreAsync(request);
        return result.Match(Ok, Problem);
    }

    [HttpPut("{genreId:guid}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> UpdateGenre(
        Guid genreId,
        UpdateGenreRequest request)
    {
        var result = await _genreService
            .UpdateGenreAsync(genreId, request);
        return result.Match(Ok, Problem);
    }

    [HttpDelete("{genreId:guid}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> DeleteGenre(
        Guid genreId)
    {
        var result = await _genreService
            .DeleteGenreAsync(genreId);
        return result.Match(Ok, Problem);
    }
}