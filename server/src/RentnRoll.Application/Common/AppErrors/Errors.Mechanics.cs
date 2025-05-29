using RentnRoll.Domain.Common;

namespace RentnRoll.Application.Common.AppErrors;

public static partial class Errors
{
    public static class Mechanics
    {
        public static Error AlreadyExists(string name) =>
            Error.InvalidRequest(
                "Mechanics.MechanicAlreadyExists",
                $"Mechanic with name \"{name}\" already exists.");

        public static Error NotFound =>
            Error.NotFound(
                "Mechanics.MechanicNotFound",
                "Mechanic does not exist.");
    }
}