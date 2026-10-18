using School.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace School.Application.DTOs.Auth
{
    public class LoginRequest
    {
        public string? PhoneNumber { get; set; }
        public string? OtpCode { get; set; }
        public string? UserName { get; set; }
        public string? Password { get; set; }
        public UserRole UserRole { get; set; }
    }
}
