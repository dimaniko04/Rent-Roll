namespace RentnRoll.Application.Contracts.Users;

public record UserResponse(
    string Id,
    string Email,
    string FullName,
    string? Country
);