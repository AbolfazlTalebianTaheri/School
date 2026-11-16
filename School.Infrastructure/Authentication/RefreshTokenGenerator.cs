using Microsoft.AspNetCore.WebUtilities;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace School.Infrastructure.Authentication
{
    public class RefreshTokenGenerator
    {
        public static string Generator()
        {
            var bytes = RandomNumberGenerator.GetBytes(64);
            return WebEncoders.Base64UrlEncode(bytes);
        }
        public static string Hash(string token)
        {
            var bytes = SHA256.HashData(Encoding.UTF8.GetBytes(token));
            return Convert.ToHexString(bytes);
        }
    }
}
