namespace RentnRoll.Application.Contracts.Authentication;

public record RefreshRequest(
    string RefreshToken,
    string AccessToken
);