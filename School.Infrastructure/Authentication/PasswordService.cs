using Microsoft.AspNetCore.Identity;
using School.Application.InterFaces;
using School.Domain.Entitys;
using System;
using System.Collections.Generic;
using System.Text;

namespace School.Infrastructure.Authentication
{
    public class PasswordService(
      IPasswordHasher<User> passwordHasher) : IPasswordService
    {
        public string HashPassword(string password)
        {
            var user = new User(
                userName: null,
                userRole: default,
                passWord: null,
                studentId: null);

            return passwordHasher.HashPassword(user, password);
        }

        public bool VerifyPassword(string password, string passwordHash)
        {
            var user = new User(
                userName: null,
                userRole: default,
                passWord: passwordHash,
                studentId: null);

            var result = passwordHasher.VerifyHashedPassword(
                user,
                passwordHash,
                password);

            return result != PasswordVerificationResult.Failed;
        }
    }
}
