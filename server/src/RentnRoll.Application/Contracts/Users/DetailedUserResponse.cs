namespace RentnRoll.Application.Contracts.Users;

public record DetailedUserResponse(
    string Id,
    string Email,
    string FullName,
    string? Country,
    DateTime CreatedAt,
    DateTime? UpdatedAt,
    DateTime? DeletedAt,
    IEnumerable<string> Roles
);