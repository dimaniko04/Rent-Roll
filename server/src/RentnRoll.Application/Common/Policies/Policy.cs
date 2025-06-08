namespace RentnRoll.Application.Common.Policies;

public static class Policy
{
    public const string CreatorOrAdmin = "CreatorOrAdmin";

    public const string OwnerOrAdmin = "OwnerOrAdmin";
    public const string OwnerOnly = "OwnerOnly";
}