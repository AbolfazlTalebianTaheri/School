using AutoMapper;
using School.Api.Contracts.Student;
using School.Application.DTOs;

namespace School.Api.Mapping.Student
{
    public class MappingStudent :Profile
    {
        public MappingStudent()
        {
            CreateMap<StudentDto, StudentResponse>();
            CreateMap<CreateStudentRequest, StudentDto>();
            CreateMap<UpdateStudentRequest, StudentDto>();


            CreateMap<StudentResponse, StudentDto>();
        }
    }
}
