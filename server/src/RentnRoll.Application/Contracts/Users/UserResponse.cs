namespace RentnRoll.Application.Contracts.Users;

public record UserResponse(
    string Id,
    string Email,
    string? Country,
    DateTime? BirthDate,
    IEnumerable<string> Roles
);