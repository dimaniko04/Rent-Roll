namespace RentnRoll.Application.Contracts.Authentication;

public record AuthResponse(
    string AccessToken,
    string RefreshToken
);