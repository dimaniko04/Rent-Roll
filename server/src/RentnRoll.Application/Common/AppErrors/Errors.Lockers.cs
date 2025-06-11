using RentnRoll.Domain.Common;

namespace RentnRoll.Application.Common.AppErrors;

public static partial class Errors
{
    public static class Lockers
    {
        public static Error AlreadyExists(string name) =>
            Error.InvalidRequest(
                "Lockers.AlreadyExists",
                $"Locker with name \"{name}\" already exists.");

        public static Error NotFound =>
            Error.NotFound(
                "Lockers.NotFound",
                "Locker is inactive or does not exist.");
    }
}