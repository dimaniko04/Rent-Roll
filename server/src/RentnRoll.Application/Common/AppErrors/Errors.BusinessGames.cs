using RentnRoll.Domain.Common;

namespace RentnRoll.Application.Common.AppErrors;

public static partial class Errors
{
    public static class BusinessGames
    {
        public static Error AlreadyExists(string name, ICollection<string> tags) =>
            Error.InvalidRequest(
                "BusinessGame.AlreadyExists",
                $"Game with name \"{name}\" and tags {string.Join(" ", tags)} already exists in catalog.");

        public static Error NotFound =>
            Error.NotFound(
                "BusinessGame.NotFound",
                "Game does not exist in business catalog.");

        public static Error IdsNotFound(ICollection<Guid> ids) =>
            Error.NotFound(
                "BusinessGame.IdsNotFound",
                $"Business games with IDs {string.Join(" ", ids)} do not exist in business catalog.");

        public static Error TagsNotFound(ICollection<string> tags) =>
            Error.NotFound(
                "BusinessGame.TagsNotFound",
                $"Tags {string.Join(" ", tags)} do not exist.");
    }
}