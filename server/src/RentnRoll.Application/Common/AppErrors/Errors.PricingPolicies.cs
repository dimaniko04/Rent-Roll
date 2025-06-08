using RentnRoll.Domain.Common;

namespace RentnRoll.Application.Common.AppErrors;

public static partial class Errors
{
    public static class PricingPolicies
    {
        public static Error AlreadyExists(string name) =>
            Error.InvalidRequest(
                "PricingPolicy.AlreadyExists",
                $"Pricing policy with name \"{name}\" already exists.");

        public static Error NotFound =>
            Error.NotFound(
                "PricingPolicy.NotFound",
                "Pricing policy does not exist.");

        public static Error GamesNotFound(IEnumerable<Guid> gameIds) =>
            Error.NotFound(
                "PricingPolicy.GamesNotFound",
                $"Games with following ids: {string.Join(", ", gameIds)} do not exist in business catalog.");
    }
}
