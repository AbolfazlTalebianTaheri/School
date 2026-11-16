using School.Api.Contracts.Lesson;
using School.Api.Contracts.Student;
using School.Domain.Enums;

namespace School.Api.Contracts.User
{
    public record class UserResponse(string? UserName, string? NationalCode, UserRole UserRole,List<LessonResponse> Lessons);
}
