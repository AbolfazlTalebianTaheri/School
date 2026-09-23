using School.Api.Contracts.Student;
using School.Domain.Enums;

namespace School.Api.Contracts.User
{
    public record class UserResponse(string? UserName, string? PhoneNumber, UserRole UserRole);
}
