using School.Domain.Enums;

namespace School.Application.DTOs.Auth
{
    public class LoginRequest
    {
        /// <summary>کد ملی دانش‌آموز (ورود والدین).</summary>
        public string? NationalCode { get; set; }
        public string? UserName { get; set; }
        public string? Password { get; set; }
        public UserRole UserRole { get; set; }
        /// <summary>true = کوکی رفرش‌توکن ماندگار (مرا به خاطر بسپار).</summary>
        public bool RememberMe { get; set; }
    }
}
