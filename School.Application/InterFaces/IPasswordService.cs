using System;
using System.Collections.Generic;
using System.Text;

namespace School.Application.InterFaces
{
    public interface IPasswordService
    {
        string HashPassword(string password);
        bool VerifyPassword(string password, string passwordHash);
    }
}
