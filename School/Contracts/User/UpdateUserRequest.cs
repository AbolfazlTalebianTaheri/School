using School.Domain.Enums;

namespace School.Api.Contracts.User
{
    public record UpdateUserRequest(string? UserName, string? NationalCode, string? Password, ushort? StudentId, UserRole UserRole);
}
