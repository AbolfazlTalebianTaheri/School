using School.Application.DTOs;
using School.Domain.Enums;

namespace School.Api.Contracts.Student
{
    public record CreateStudentRequest(string FirstName, string LastName, int StudentCode,Grade_Level Grade_Level, Field_Of_Study Field_Of_Study);
}
