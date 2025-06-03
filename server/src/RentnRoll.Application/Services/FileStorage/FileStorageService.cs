
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

using RentnRoll.Application.Common.AppErrors;
using RentnRoll.Application.Common.Options;
using RentnRoll.Domain.Common;

namespace RentnRoll.Application.Services.FileStorage;

public class FileStorageService : IFileStorageService
{
    private readonly string _uploadRoot;
    private readonly string _uploadSubFolder;

    public FileStorageService(
        IOptions<FileStorageOptions> options)
    {
        _uploadRoot = options.Value.UploadRoot;
        _uploadSubFolder = options.Value.UploadSubFolder;
    }

    public async Task<Result<string>> UploadAsync(
        IFormFile file,
        string folder,
        string? fileName = null,
        long maxFileSize = 5 * 1024 * 1024, // 5 MB default
        string[]? allowedExtensions = null)
    {
        if (file == null || file.Length == 0)
            return Errors.Files.EmptyFile;

        if (file.Length > maxFileSize)
            return Errors.Files.TooLarge(maxFileSize);

        var extension = Path.GetExtension(file.FileName).ToLowerInvariant();
        if (allowedExtensions != null && allowedExtensions.Length > 0)
        {
            if (!allowedExtensions.Contains(extension))
                return Errors.Files.InvalidFileType(extension);
        }

        var uploadFolder = Path.Combine(
            _uploadRoot,
            folder);
        Directory.CreateDirectory(uploadFolder);

        var name = fileName ?? $"{Guid.NewGuid()}_{DateTime.UtcNow:yyyy-MM-ddTHH:mm:ss}{Path.GetExtension(file.FileName)}";
        var filePath = Path.Combine(uploadFolder, name);

        using var stream = new FileStream(filePath, FileMode.Create);
        await file.CopyToAsync(stream);

        return $"{_uploadSubFolder}/{folder}/{name}";
    }

    public Result Delete(string filePath)
    {
        var fullPath = Path.Combine(_uploadRoot, filePath);

        if (!File.Exists(fullPath))
            return Result.Failure(
                [Errors.Files.NotFound(filePath)]);

        File.Delete(fullPath);

        return Result.Success();
    }
}
