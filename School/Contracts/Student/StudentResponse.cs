using School.Application.DTOs;
using School.Domain.Entitys;
using School.Domain.Enums;

namespace School.Api.Contracts.Student
{
    public record StudentResponse(string FirstName, string LastName, int StudentCode, List<UserDto> Users , Grade_Level Grade_Level, Field_Of_Study Field_Of_Study);
}
