using System;
using System.Collections.Generic;
using System.Text;

namespace School.Application.InterFaces
{
    public interface ITokenService
    {
        string GenerateAccessToken(string userId, string userName, string role);
    }
}
