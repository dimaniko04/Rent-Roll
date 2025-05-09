namespace RentnRoll.Application.Contracts.Authentication;

public record UserLoginRequest(
    string Email,
    string Password
);