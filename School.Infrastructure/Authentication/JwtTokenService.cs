using Microsoft.Extensions.Options;
//using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using School.Application.InterFaces;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace School.Infrastructure.Authentication
{
    public class JwtTokenService(IOptions<JwtSettings> options) : ITokenService
    {
        private readonly JwtSettings _setting = options.Value;
        public string GenerateAccessToken(string userId, string userName, string role)
        {
            var claims = new List<Claim>()
            {
                new Claim(JwtRegisteredClaimNames.Sub,userId),
                new Claim(JwtRegisteredClaimNames.UniqueName,userName),
                new Claim(ClaimTypes.Role,role),
                new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString())
            };
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
    }
}
