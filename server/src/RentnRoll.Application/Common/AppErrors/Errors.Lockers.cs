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

        public static Error DeviceNotConfigured(
            string deviceId) =>
            Error.InvalidRequest(
                "Lockers.DeviceNotConfigured",
                $"Device with ID {deviceId} is not configured.");

        public static Error CellsNotEmpty(ICollection<Guid> guids) =>
            Error.InvalidRequest(
                "Lockers.CellInUse",
                $"The following cells are not empty: {string.Join(", ", guids)}. " +
                "Please remove the items from these cells before proceeding."
        );

        public static Error NotEnoughCells(
            Guid lockerId,
            int requiredCells,
            int availableCells) =>
            Error.InvalidRequest(
                "Lockers.NotEnoughCells",
                $"Locker with ID {lockerId} has not enough available cells. Required: {requiredCells}, Available: {availableCells}.");

        public static Error DeviceAlreadyConfigured(
            string deviceId) =>
            Error.InvalidRequest(
                "Lockers.DeviceAlreadyConfigured",
                $"Device with ID {deviceId} is already configured. " +
                "Please delete the existing configuration before creating a new one.");
    }
}