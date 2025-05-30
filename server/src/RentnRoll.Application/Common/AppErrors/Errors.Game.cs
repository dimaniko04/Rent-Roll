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
    }
}