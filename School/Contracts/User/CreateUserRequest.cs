using School.Domain.Enums;

namespace School.Api.Contracts.User
{
    public record CreateUserRequest(UserRole UserRole,string? UserName =null, string? Password = null, string? PhoneNumber = null, ushort? StudentId =null);
}
