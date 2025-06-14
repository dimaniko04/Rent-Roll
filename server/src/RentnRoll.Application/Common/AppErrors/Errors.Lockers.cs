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
                "Locker is deactivated or does not exist.");

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
                $"Locker with ID {lockerId} has not enough available cells." +
                $"Required: {requiredCells}, Available: {availableCells}.");

        public static Error CannotDeleteActiveCells(
            ICollection<Guid> cellIds) =>
            Error.InvalidRequest(
                "Lockers.CannotDeleteActiveCells",
                $"Cannot delete active cells with IDs: {string.Join(", ", cellIds)}. " +
                "Please remove all cell configurations and assignments before deleting them.");

        public static Error HasActiveRentals(
            Guid lockerId) =>
            Error.InvalidRequest(
                "Lockers.HasActiveRentals",
                $"Cannot delete active locker with ID {lockerId}. " +
                "Please ensure there are no active rentals associated with this locker.");

        public static Error DeviceAlreadyConfigured(
            string deviceId) =>
            Error.InvalidRequest(
                "Lockers.DeviceAlreadyConfigured",
                $"Device with ID {deviceId} is already configured. " +
                "Please delete the existing configuration before creating a new one.");

        public static Error CellsNotFound(
            ICollection<Guid> cellIds) =>
            Error.NotFound(
                "Lockers.CellsNotFound",
                $"Cells with IDs {string.Join(", ", cellIds)} not found. ");

        public static Error CellsNotBelongToBusiness(
            Guid businessId,
            ICollection<Guid> cellIds) =>
            Error.InvalidRequest(
                "Lockers.CellsNotBelongToBusiness",
                $"Cells with IDs {string.Join(", ", cellIds)} do" +
                $"not belong to the business with ID {businessId}.");
    }
}