using Microsoft.AspNetCore.Http;

using RentnRoll.Domain.Common;

namespace RentnRoll.Application.Services.FileStorage;

public interface IFileStorageService
{
    Task<Result<string>> UploadAsync(
        IFormFile file,
        string folder,
        string? fileName = null,
        long maxFileSize = 5 * 1024 * 1024, // 5 MB default
        string[]? allowedExtensions = null);
    Result Delete(string filePath);
}