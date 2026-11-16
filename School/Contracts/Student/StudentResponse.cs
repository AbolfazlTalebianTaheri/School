using School.Application.DTOs.User;
using School.Domain.Entitys;
using School.Domain.Enums;

namespace School.Api.Contracts.Student
{
    public record StudentResponse(ushort Id, string FirstName, string LastName, int StudentCode, string NationalCode,List<UserRsponseDto> Users , Grade_Level Grade_Level, Field_Of_Study Field_Of_Study);
}
