using RentnRoll.Application.Contracts.Genres;
using RentnRoll.Domain.Common;

namespace RentnRoll.Application.Services.Genres;

public interface IGenreService
{
    Task<Result<GenreResponse>> CreateGenreAsync(
        CreateGenreRequest request);

    Task<Result<GenreResponse>> UpdateGenreAsync(
        Guid GenreId,
        UpdateGenreRequest request);

    Task<Result<GenreResponse>> GetGenreByIdAsync(
        Guid GenreId);

    Task<IEnumerable<GenreResponse>> GetAllGenresAsync(
        GetAllGenresRequest request);

    Task<Result> DeleteGenreAsync(Guid GenreId);
}