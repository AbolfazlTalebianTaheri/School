using System;
using System.Collections.Generic;
using System.Text;

namespace School.Application.DTOs.Auth
{
    public record RefreshTokenResponce(string AccessToken, DateTime AccessTokenExpiresAt);
}
