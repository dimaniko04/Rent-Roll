using RentnRoll.Domain.Common;

namespace RentnRoll.Application.Common.AppErrors;

public static partial class Errors
{
    public static class Games
    {
        public static Error AlreadyExists(string name) =>
            Error.InvalidRequest(
                "Game.GenreAlreadyExists",
                $"Game with name \"{name}\" already exists.");

        public static Error NotFound =>
            Error.NotFound(
                "Game.GenreNotFound",
                "Game does not exist.");

        public static Error ImagesNotFound(List<string> imagePaths) =>
            Error.InvalidRequest(
                "Game.ImagesNotFound",
                $"Images {string.Join(", ", imagePaths)} do not exist.");

        public static Error GenresNotFound(List<string> genres) =>
            Error.InvalidRequest(
                "Game.GenresNotFound",
                $"Genres {string.Join(", ", genres)} do not exist.");

        public static Error CategoriesNotFound(List<string> categories) =>
            Error.InvalidRequest(
                "Game.CategoriesNotFound",
                $"Categories {string.Join(", ", categories)} do not exist.");

        public static Error MechanicsNotFound(List<string> mechanics) =>
            Error.InvalidRequest(
                "Game.MechanicsNotFound",
                $"Mechanics {string.Join(", ", mechanics)} do not exist.");
    }
}