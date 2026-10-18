using Microsoft.AspNetCore.Identity;
using School.Application.DTOs.Auth;
using School.Application.InterFaces;
using School.Application.Repository;
using School.Domain.Entitys;
using System;
using System.Collections.Generic;
using System.Text;

namespace School.Application.Service.Auth
{
    public class AuthService(IUserRepository userRepository,
    IPasswordHasher<User> passwordHasher,
    ITokenService tokenService)
    {
        public async Task<LoginResponse?> LoginAsync(LoginRequest request, CancellationToken cancellationToken = default)
        {
            if (!string.IsNullOrWhiteSpace(request.PhoneNumber) ||
                !string.IsNullOrWhiteSpace(request.OtpCode))
                return null;
            var user = await userRepository.GetByUserNameAsync(request.UserName, cancellationToken);
            if (string.IsNullOrWhiteSpace(user.PassWordHash))
                return null;
            if(user.UserRole != request.UserRole)
                return null;
            var passwordResult = passwordHasher.VerifyHashedPassword(user, user.PassWordHash, request.Password);
            //if (passwordResult == PasswordVerificationResult.Failed)
            //    return null; // فعلا پیاده سازی نشده
            var accessToken = tokenService.GenerateAccessToken(user.Id.ToString(), user.UserName ?? user.PhoneNumber!, user.UserRole.ToString());
            return new LoginResponse
            {
                UserId = user.Id,
                AccessToken = accessToken
            };
        }
        public async Task<LoginResponse?> LoginWithOtpAsync(LoginRequest loginRequest, CancellationToken cancellationToken = default)
        {
            return null;// Not Impelement
        }
    }
}
