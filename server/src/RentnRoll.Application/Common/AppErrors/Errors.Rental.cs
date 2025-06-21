using RentnRoll.Domain.Common;

namespace RentnRoll.Application.Common.AppErrors;

public static partial class Errors
{
    public static class Rentals
    {
        public static Error CellPolicyNotFound(Guid cellId, string unit) =>
            Error.NotFound(
                "Rentals.CellPolicyNotFound",
                $"Cell policy for cell with ID {cellId} and time unit {unit} does not exist.");

        public static Error AssetPolicyNotFound(Guid storeAssetId, string unit) =>
            Error.NotFound(
                "Rentals.AssetPolicyNotFound",
                $"Asset policy for store asset with ID {storeAssetId} and time unit {unit} does not exist.");

        public static Error RentalNotFound(Guid rentalId) =>
            Error.NotFound(
                "Rentals.RentalNotFound",
                $"Rental with ID {rentalId} does not exist.");

        public static Error RentalAlreadyActive(Guid rentalId) =>
            Error.InvalidRequest(
                "Rentals.RentalAlreadyActive",
                $"Rental with ID {rentalId} cannot be canceled because it is already active.");

        public static Error RentalNotActive(Guid rentalId) =>
            Error.InvalidRequest(
                "Rentals.RentalNotActive",
                $"Rental with ID {rentalId} cannot be returned because it is not active.");

        public static Error NoRelatedStore(Guid guid) =>
            Error.NotFound(
                "Rentals.NoRelatedStore",
                $"No store is related to the rental with ID {guid}.");

        public static Error NoRelatedCell(Guid guid) =>
            Error.NotFound(
                "Rentals.NoRelatedStore",
                $"No cell is related to the rental with ID {guid}.");

        public static Error CellNotInMaintenance(Guid cellId) =>
            Error.InvalidRequest(
                "Rentals.CellNotInMaintenance",
                $"Cell with ID {cellId} is not in maintenance.");
    }
}