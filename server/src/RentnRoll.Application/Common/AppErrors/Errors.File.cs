using RentnRoll.Domain.Common;

namespace RentnRoll.Application.Common.AppErrors;

public static partial class Errors
{
    public static class Files
    {
        public static Error EmptyFile =>
            Error.InvalidRequest(
                "Files.EmptyFile",
                "File cannot be empty.");

        public static Error TooLarge(long maxSize) =>
            Error.InvalidRequest(
                "Files.TooLarge",
                $"File exceeds the maximum allowed size of {maxSize} bytes.");

        public static Error InvalidFileType(string fileType) =>
            Error.Unsupported(
                "Files.InvalidFileType",
                $"File type \"{fileType}\" is not supported.");

        public static Error NotFound(string filePath) =>
            Error.NotFound(
                "Files.FileNotFound",
                $"File with path \"{filePath}\" does not exist.");
    }
}