namespace RentnRoll.Application.Contracts.Users;

public record UserLoginRequest(
    string Email,
    string Password
);
