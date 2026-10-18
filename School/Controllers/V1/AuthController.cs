using Microsoft.AspNetCore.Mvc;
using School.Api.Constants;
using School.Api.Contracts;
using School.Application.DTOs.Auth;
using School.Application.Service.Auth;

namespace School.Api.Controllers.V1
{
    public class AuthController(AuthService authService) : BaseController
    {
        [HttpPost(AuthUriContrcts.Login)]
        [ProducesResponseType(typeof(ApiResult<LoginResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResult<LoginResponse>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResult<LoginResponse>), StatusCodes.Status401Unauthorized)]
        public async Task<ApiResult<LoginResponse>> Login([FromBody] LoginRequest loginRequest, CancellationToken cancellationToken = default)
        {
            LoginResponse response;
            if (!string.IsNullOrWhiteSpace(loginRequest.OtpCode))
            {
                // پیاده سازی کد Otp   
                response = await authService.LoginWithOtpAsync(loginRequest, cancellationToken);
            }
            else if (!string.IsNullOrWhiteSpace(loginRequest.Password))
            {
                response = await authService.LoginAsync(loginRequest, cancellationToken);
            }
            else
                return ApiResult<LoginResponse>.Failed(new { Message = "اطلاعات ناقص است" }, StatusCodes.Status400BadRequest);
            if (response is null)
                return ApiResult<LoginResponse>.Failed(new { message = "اطلاعات ورود نامعتبر است." }, StatusCodes.Status401Unauthorized);

            return ApiResult<LoginResponse>.Succeeded(response);
        }
    }
}
