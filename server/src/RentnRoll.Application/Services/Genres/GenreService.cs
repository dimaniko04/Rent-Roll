using RentnRoll.Application.Common.Interfaces.Repositories;
using RentnRoll.Application.Contracts.Genres;
using RentnRoll.Application.Specifications.Genres;
using RentnRoll.Domain.Common;
using RentnRoll.Application.Common.AppErrors;
using RentnRoll.Application.Services.Validation;
using RentnRoll.Application.Common.Interfaces.UnitOfWork;
using Microsoft.Extensions.Logging;

namespace RentnRoll.Application.Services.Genres;

public class GenreService : IGenreService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IGenreRepository _GenreRepository;
    private readonly IValidationService _validationService;

    public GenreService(
        IUnitOfWork unitOfWork,
        ILogger<GenreService> logger,
        IValidationService validationService)
    {
        _unitOfWork = unitOfWork;
        _validationService = validationService;
        _GenreRepository = unitOfWork
            .GetRepository<IGenreRepository>();
    }

    public async Task<IEnumerable<GenreResponse>> GetAllGenresAsync(
        GetAllGenresRequest request)
    {
        var specification = new GetAllGenresRequestSpec(request);

        var Genres = await _GenreRepository
            .GetAllAsync(specification);
        var GenreResponses = Genres
            .Select(GenreResponse.FromGenre);

        return GenreResponses;
    }

    public async Task<Result<GenreResponse>> GetGenreByIdAsync(
        Guid GenreId)
    {
        var Genre = await _GenreRepository.GetByIdAsync(GenreId);

        if (Genre == null)
            return Errors.Genres.NotFound;

        var genreResponse = GenreResponse
            .FromGenre(Genre);

        return genreResponse;
    }

    public async Task<Result<GenreResponse>> CreateGenreAsync(
        CreateGenreRequest request)
    {
        var validationResult = await _validationService
            .ValidateAsync(request);
        if (validationResult.IsError)
            return validationResult.Errors;

        var existingGenre = await _GenreRepository
            .GetByNameAsync(request.Name);

        if (existingGenre != null)
            return Errors.Genres.AlreadyExists(request.Name);

        var Genre = request.ToGenre();
        await _GenreRepository.CreateAsync(Genre);
        await _unitOfWork.SaveChangesAsync();

        var genreResponse = GenreResponse
            .FromGenre(Genre);

        return genreResponse;
    }

    public async Task<Result<GenreResponse>> UpdateGenreAsync(
        Guid id,
        UpdateGenreRequest request)
    {
        var validationResult = await _validationService
                    .ValidateAsync(request);
        if (validationResult.IsError)
            return validationResult.Errors;

        var Genre = await _GenreRepository
            .GetByIdAsync(id);

        if (Genre == null)
            return Errors.Genres.NotFound;

        Genre.Name = request.Name;
        _GenreRepository.Update(Genre);
        await _unitOfWork.SaveChangesAsync();

        var genreResponse = GenreResponse
            .FromGenre(Genre);

        return genreResponse;
    }

    public async Task<Result> DeleteGenreAsync(Guid GenreId)
    {
        var Genre = await _GenreRepository.GetByIdAsync(GenreId);
        if (Genre == null)
            return Result.Failure([Errors.Genres.NotFound]);

        _GenreRepository.Delete(Genre);
        await _unitOfWork.SaveChangesAsync();

        return Result.Success();
    }
}