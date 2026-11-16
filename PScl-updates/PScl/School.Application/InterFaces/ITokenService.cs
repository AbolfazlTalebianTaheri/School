using System;
using System.Collections.Generic;

namespace School.Application.InterFaces
{
    public interface ITokenService
    {
        /// <param name="studentIds">دانش‌آموزهای وصل‌شده به حساب والد (در توکن به‌صورت student_id می‌آید).</param>
        string GenerateAccessToken(string userId, string userName, string role, IEnumerable<ushort>? studentIds = null);
        string GenerateRefreshToken();
        string HashRefreshToken(string refreshToken);
        DateTime GetRefreshTokenExpiresAtUtc(bool isPersistent);
        /// <summary>اگر رفرش‌توکن همین الان چرخانده شده باشد (چند تب هم‌زمان)، در این بازه دوباره پذیرفته می‌شود.</summary>
        TimeSpan RefreshReuseLeeway { get; }
    }
}
