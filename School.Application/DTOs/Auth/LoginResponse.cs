using System;
using System.Collections.Generic;
using System.Text;

namespace School.Application.DTOs.Auth
{
    public class LoginResponse
    {
        public ushort UserId { get; set; }
        public string AccessToken { get; set; } = string.Empty;
    }
}
