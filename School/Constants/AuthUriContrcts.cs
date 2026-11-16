namespace School.Api.Constants
{
    public static class AuthUriContrcts
    {
        private const string Controller = "auth";
        public const string Login = $"{Controller}/Login";
        public const string Refresh = $"{Controller}/Refresh";
        public const string Logout = $"{Controller}/Logout";
        public const string LogoutAll = $"{Controller}/LogoutAll";
        public const string OtpRequest = $"{Controller}/otp/request";
        public const string OtpVerify = $"{Controller}/otp/verify";
    }
}
