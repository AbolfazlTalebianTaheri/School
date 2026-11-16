namespace School.Application.DTOs.Auth
{
    public sealed record AuthResult(
        LoginResponse Response,
        string RefreshToken,
        DateTime RefreshTokenExpiresAtUtc,
        bool IsPersistent);
}
