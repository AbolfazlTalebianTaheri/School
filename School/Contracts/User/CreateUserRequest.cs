using School.Domain.Enums;

namespace School.Api.Contracts.User
{
    public record CreateUserRequest(string? UserName, string? PhoneNumber, string? Password, ushort? StudentId, UserRole UserRole);
}
