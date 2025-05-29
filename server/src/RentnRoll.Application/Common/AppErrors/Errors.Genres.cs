using RentnRoll.Domain.Common;

namespace RentnRoll.Application.Common.AppErrors;

public static partial class Errors
{
    public static class Genres
    {
        public static Error AlreadyExists(string name) =>
            Error.InvalidRequest(
                "Genres.GenreAlreadyExists",
                $"Genre with name \"{name}\" already exists.");

        public static Error NotFound =>
            Error.NotFound(
                "Genres.GenreNotFound",
                "Genre does not exist.");
    }
}