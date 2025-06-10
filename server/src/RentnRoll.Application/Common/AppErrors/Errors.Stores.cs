using RentnRoll.Domain.Common;

namespace RentnRoll.Application.Common.AppErrors;

public static partial class Errors
{
    public static class Stores
    {
        public static Error NotFound =>
            Error.NotFound(
                "Stores.StoreNotFound",
                "Store not found.");

        public static Error AlreadyExists =>
            Error.InvalidRequest(
                "Stores.StoreAlreadyExists",
                "Store with the same name already exists.");

        public static Error PolicyNotFound(Guid id) =>
            Error.NotFound(
                "Stores.PolicyNotFound",
                $"Policy with id: ${id} does not exist in business catalog.");

        public static Error GamesNotFound(IEnumerable<Guid> gameIds) =>
            Error.NotFound(
                "Stores.GamesNotFound",
                $"Games with following ids: {string.Join(", ", gameIds)} do not exist in business catalog.");
    }
}