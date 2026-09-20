using School.Api.Contracts.Student;
using School.Domain.Enums;

namespace School.Api.Contracts.User
{
    public record class UserResponse(string? UserName, string? PhoneNumber, string? PassWordHash, ushort? StudentId, StudentResponse? Student, UserRole UserRole);
}
