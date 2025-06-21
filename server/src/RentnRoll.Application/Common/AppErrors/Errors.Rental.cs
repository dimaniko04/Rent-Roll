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
    }
}