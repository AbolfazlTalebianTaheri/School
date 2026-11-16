using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.Extensions.Options;
using School.Api.Constants;
using School.Api.Contracts;
using School.Application.DTOs.Auth;
using School.Application.Service.Auth;
using School.Domain.Enums;
using School.Infrastructure.Authentication;

namespace School.Api.Controllers.V1
{
    public class AuthController(
        AuthService authService,
        IConfiguration configuration,
        IOptions<JwtSettings> jwtSettings) : BaseController
    {
        private string CookieName => configuration["RefreshCookie:Name"] ?? "refresh_token";

        private CookieOptions BuildCookieOptions() => new()
        {
            HttpOnly = true,
            Secure = configuration.GetValue("RefreshCookie:Secure", true),
            SameSite = Enum.TryParse<SameSiteMode>(configuration["RefreshCookie:SameSite"], true, out var sameSite)
                ? sameSite
                : SameSiteMode.Strict,
            Path = configuration["RefreshCookie:Path"] ?? "/api/v1/auth"
        };

        private void SetRefreshCookie(AuthResult result)
        {
            var options = BuildCookieOptions();
            // مرا به خاطر بسپار = کوکی ماندگار؛ در غیر این صورت با بسته شدن مرورگر پاک می‌شود
            options.Expires = result.IsPersistent ? new DateTimeOffset(result.RefreshTokenExpiresAtUtc, TimeSpan.Zero) : null;
            Response.Cookies.Append(CookieName, result.RefreshToken, options);
        }

        [AllowAnonymous]
        [EnableRateLimiting("login")]
        [HttpPost(AuthUriContrcts.Login)]
        [ProducesResponseType(typeof(ApiResult<LoginResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResult<LoginResponse>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResult<LoginResponse>), StatusCodes.Status401Unauthorized)]
        public async Task<ApiResult<LoginResponse>> Login([FromBody] LoginRequest loginRequest, CancellationToken cancellationToken = default)
        {
            var hasCredentials = loginRequest.UserRole == UserRole.Parent
                ? !string.IsNullOrWhiteSpace(loginRequest.NationalCode)
                : !string.IsNullOrWhiteSpace(loginRequest.UserName) && !string.IsNullOrEmpty(loginRequest.Password);

            if (!hasCredentials)
                return ApiResult<LoginResponse>.Failed(new { message = "اطلاعات ناقص است." }, StatusCodes.Status400BadRequest);

            var result = await authService.LoginAsync(loginRequest, cancellationToken);
            if (result is null)
            {
                return ApiResult<LoginResponse>.Failed(
                    new { message = "اطلاعات ورود نامعتبر است." },
                    StatusCodes.Status401Unauthorized);
            }

            SetRefreshCookie(result);
            return ApiResult<LoginResponse>.Succeeded(result.Response);
        }

        [AllowAnonymous]
        [HttpPost(AuthUriContrcts.Refresh)]
        public async Task<ApiResult<RefreshTokenResponce>> Refresh(CancellationToken cancellationToken = default)
        {
            Request.Cookies.TryGetValue(CookieName, out var refreshToken);

            var result = await authService.RefreshAsync(refreshToken, cancellationToken);
            if (result is null)
            {
                Response.Cookies.Delete(CookieName, BuildCookieOptions());
                return ApiResult<RefreshTokenResponce>.Failed(
                    new { message = "نشست شما منقضی شده است. دوباره وارد شوید." },
                    StatusCodes.Status401Unauthorized);
            }

            if (!string.IsNullOrEmpty(result.RefreshToken))
                SetRefreshCookie(result);

            return ApiResult<RefreshTokenResponce>.Succeeded(new RefreshTokenResponce(
                result.Response.AccessToken,
                DateTime.UtcNow.AddMinutes(jwtSettings.Value.ExpiratinMinutes)));
        }

        [AllowAnonymous]
        [HttpPost(AuthUriContrcts.Logout)]
        public async Task<ApiResult> Logout(CancellationToken cancellationToken = default)
        {
            if (Request.Cookies.TryGetValue(CookieName, out var refreshToken))
                await authService.LogoutAsync(refreshToken, cancellationToken);

            Response.Cookies.Delete(CookieName, BuildCookieOptions());
            return ApiResult.NoContent();
        }

        [HttpPost(AuthUriContrcts.LogoutAll)]
        public async Task<ApiResult> LogoutAll(CancellationToken cancellationToken = default)
        {
            if (CurrentUserId is ushort userId)
                await authService.LogoutAllAsync(userId, cancellationToken);

            Response.Cookies.Delete(CookieName, BuildCookieOptions());
            return ApiResult.NoContent();
        }
    }
}
