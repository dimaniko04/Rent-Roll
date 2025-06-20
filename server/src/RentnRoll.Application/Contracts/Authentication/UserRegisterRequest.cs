namespace RentnRoll.Application.Contracts.Authentication;

public record UserRegisterRequest(
    string FullName,
    string Email,
    string Country,
    string Password
);