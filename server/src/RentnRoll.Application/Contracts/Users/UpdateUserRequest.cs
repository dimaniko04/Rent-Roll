namespace RentnRoll.Application.Contracts.Users;

public record UpdateUserRequest(
    string Email,
    string Country,
    string FullName
);