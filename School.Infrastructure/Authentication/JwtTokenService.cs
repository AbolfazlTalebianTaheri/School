using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using School.Application.InterFaces;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace School.Infrastructure.Authentication
{
    public class JwtTokenService(IOptions<JwtSettings> options, IOptions<RefreshTokenSettings> refreshOptions) : ITokenService
    {
        private readonly JwtSettings _setting = options.Value;
        private readonly RefreshTokenSettings _refresh = refreshOptions.Value;

        public TimeSpan RefreshReuseLeeway => TimeSpan.FromSeconds(_refresh.ReuseLeewaySeconds);

        public string GenerateAccessToken(string userId, string userName, string role, IEnumerable<ushort>? studentIds = null)
        {
            var claims = new List<Claim>()
            {
                new Claim(JwtRegisteredClaimNames.Sub,userId),
                new Claim(JwtRegisteredClaimNames.UniqueName,userName),
                new Claim(ClaimTypes.Role,role),
                new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString())
            };
            if (studentIds is not null)
            {
                foreach (var studentId in studentIds)
                    claims.Add(new Claim("student_id", studentId.ToString()));
            }

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_setting.SecretKey));
            var credentials =new SigningCredentials(key,SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(
                issuer: _setting.Issuer,
                audience: _setting.Audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(_setting.ExpiratinMinutes),
                signingCredentials: credentials
                );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public string GenerateRefreshToken() =>
            Base64UrlEncoder.Encode(RandomNumberGenerator.GetBytes(64));

        public string HashRefreshToken(string refreshToken) =>
            Convert.ToHexString(SHA256.HashData(Encoding.UTF8.GetBytes(refreshToken)));

        public DateTime GetRefreshTokenExpiresAtUtc(bool isPersistent) =>
            isPersistent
                ? DateTime.UtcNow.AddDays(_refresh.PersistentLifetimeDays)
                : DateTime.UtcNow.AddHours(_refresh.SessionLifetimeHours);
    }
}
