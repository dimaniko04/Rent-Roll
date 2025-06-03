using System.Collections.ObjectModel;

using Microsoft.AspNetCore.Http;

namespace RentnRoll.Application.Contracts.Games.ReplaceGameImages;

public record ReplaceGameImagesRequest(
    Collection<IFormFile> Files,
    Collection<string> UnmodifiedImagePaths
);