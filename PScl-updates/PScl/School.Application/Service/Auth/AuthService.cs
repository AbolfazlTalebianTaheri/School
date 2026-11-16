using Microsoft.AspNetCore.Identity;
using School.Application.DTOs.Auth;
using School.Application.InterFaces;
using School.Application.Repository;
using School.Domain.Entitys;
using School.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace School.Application.Service.Auth
{
    public class AuthService(
        IUserRepository userRepository,
        IRefreshTokenRepository refreshTokenRepository,
        IPasswordHasher<User> passwordHasher,
        ITokenService tokenService)
    {
        /// <summary>
        /// مدیر/معلم: نام کاربری + رمز عبور — والدین: کد ملی دانش‌آموز.
        /// در صورت موفقیت access token + رفرش‌توکن جدید برمی‌گرداند.
        /// </summary>
        public async Task<AuthResult?> LoginAsync(LoginRequest request, CancellationToken cancellationToken = default)
        {
            User? user;

            if (request.UserRole == UserRole.Parent)
            {
                var nationalCode = (request.NationalCode ?? string.Empty).Trim();
                if (!Regex.IsMatch(nationalCode, @"^\d{10}$"))
                    return null;

                user = await userRepository.GetByRoleAndNationalCodeAsync(UserRole.Parent, nationalCode, cancellationToken);
            }
            else
            {
                var userName = request.UserName?.Trim();
                if (string.IsNullOrEmpty(userName) || string.IsNullOrEmpty(request.Password))
                    return null;

                user = await userRepository.GetByRoleAndUserNameAsync(request.UserRole, userName, cancellationToken);
                if (user is null || string.IsNullOrWhiteSpace(user.PassWordHash))
                    return null;
                if (!VerifyPassword(user, request.Password))
                    return null;
            }

            if (user is null || user.IsDeleted)
                return null;

            return await IssueAsync(user, request.RememberMe, cancellationToken);
        }

        /// <summary>
        /// رفرش‌توکن را می‌چرخاند (rotation): توکن قبلی باطل و توکن تازه ساخته می‌شود.
        /// اگر توکن همین الان چرخانده شده باشد (چند تب هم‌زمان) فقط access token جدید می‌دهد.
        /// </summary>
        public async Task<AuthResult?> RefreshAsync(string? refreshToken, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(refreshToken))
                return null;

            var stored = await refreshTokenRepository.GetByHashAsync(tokenService.HashRefreshToken(refreshToken), cancellationToken);
            if (stored is null || stored.IsExpired)
                return null;

            if (stored.IsRevoked)
            {
                if (stored.ReplacedByTokenHash is null)
                    return null; // با خروج باطل شده

                var withinLeeway = stored.RevokedAt.HasValue &&
                    DateTime.UtcNow - stored.RevokedAt.Value <= tokenService.RefreshReuseLeeway;
                if (!withinLeeway)
                {
                    // استفاده‌ی دوباره از توکن چرخانده‌شده = احتمال سرقت؛ همه‌ی نشست‌های کاربر بسته می‌شود
                    await refreshTokenRepository.RevokeAllForUserAsync(stored.UserId, cancellationToken);
                    await refreshTokenRepository.SaveChangesAsync(cancellationToken);
                    return null;
                }

                var concurrentUser = await userRepository.GetByIdAsync(stored.UserId, cancellationToken);
                if (concurrentUser is null || concurrentUser.IsDeleted)
                    return null;

                return new AuthResult(
                    new LoginResponse { UserId = concurrentUser.Id, AccessToken = await CreateAccessTokenAsync(concurrentUser, cancellationToken) },
                    string.Empty, // کوکی تغییر نمی‌کند (تب دیگر قبلاً کوکی جدید را گرفته)
                    stored.ExpiresAt,
                    stored.IsPersistent);
            }

            var user = await userRepository.GetByIdAsync(stored.UserId, cancellationToken);
            if (user is null || user.IsDeleted)
            {
                stored.Revoke();
                await refreshTokenRepository.SaveChangesAsync(cancellationToken);
                return null;
            }

            var newRefreshToken = tokenService.GenerateRefreshToken();
            var newHash = tokenService.HashRefreshToken(newRefreshToken);
            var expiresAt = tokenService.GetRefreshTokenExpiresAtUtc(stored.IsPersistent);

            stored.Revoke(newHash);
            await refreshTokenRepository.AddAsync(new RefreshToken(newHash, user.Id, expiresAt, stored.IsPersistent), cancellationToken);
            await refreshTokenRepository.SaveChangesAsync(cancellationToken);

            return new AuthResult(
                new LoginResponse { UserId = user.Id, AccessToken = await CreateAccessTokenAsync(user, cancellationToken) },
                newRefreshToken,
                expiresAt,
                stored.IsPersistent);
        }

        public async Task LogoutAsync(string? refreshToken, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(refreshToken))
                return;

            var stored = await refreshTokenRepository.GetByHashAsync(tokenService.HashRefreshToken(refreshToken), cancellationToken);
            if (stored is null || stored.IsRevoked)
                return;

            stored.Revoke();
            await refreshTokenRepository.SaveChangesAsync(cancellationToken);
        }

        public async Task LogoutAllAsync(ushort userId, CancellationToken cancellationToken = default)
        {
            await refreshTokenRepository.RevokeAllForUserAsync(userId, cancellationToken);
            await refreshTokenRepository.SaveChangesAsync(cancellationToken);
        }

        private bool VerifyPassword(User user, string password)
        {
            try
            {
                return passwordHasher.VerifyHashedPassword(user, user.PassWordHash!, password) != PasswordVerificationResult.Failed;
            }
            catch (FormatException)
            {
                return false; // رمزی که قبلاً بدون هش ذخیره شده
            }
        }

        private async Task<AuthResult> IssueAsync(User user, bool isPersistent, CancellationToken cancellationToken)
        {
            var accessToken = await CreateAccessTokenAsync(user, cancellationToken);
            var refreshToken = tokenService.GenerateRefreshToken();
            var expiresAt = tokenService.GetRefreshTokenExpiresAtUtc(isPersistent);

            await refreshTokenRepository.AddAsync(
                new RefreshToken(tokenService.HashRefreshToken(refreshToken), user.Id, expiresAt, isPersistent),
                cancellationToken);
            await refreshTokenRepository.SaveChangesAsync(cancellationToken);

            return new AuthResult(
                new LoginResponse { UserId = user.Id, AccessToken = accessToken },
                refreshToken,
                expiresAt,
                isPersistent);
        }

        private async Task<string> CreateAccessTokenAsync(User user, CancellationToken cancellationToken)
        {
            if (user.UserRole != UserRole.Parent)
                return tokenService.GenerateAccessToken(user.Id.ToString(), user.UserName ?? $"user-{user.Id}", user.UserRole.ToString());

            var students = await userRepository.GetStudentsAsync(user.Id, cancellationToken);
            var displayName = students.Count > 0
                ? $"والد {students[0].FirstName} {students[0].LastName}"
                : $"parent-{user.Id}";

            return tokenService.GenerateAccessToken(
                user.Id.ToString(),
                displayName,
                user.UserRole.ToString(),
                students.Select(s => s.Id));
        }
    }
}
