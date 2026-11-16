using School.Domain.Enums;

namespace School.Api.Contracts.Student
{
    public record UpdateStudentRequest(string FirstName, string LastName, int StudentCode, string NationalCode, Grade_Level grade_Level, Field_Of_Study Field_Of_Study);
}
