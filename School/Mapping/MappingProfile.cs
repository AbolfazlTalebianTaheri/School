using AutoMapper;
using School.Api.Contracts.Attendance.AttendanceDay;
using School.Api.Contracts.Attendance.AttendanceDelay;
using School.Api.Contracts.Attendance.AttendanceLog;
using School.Api.Contracts.Lesson;
using School.Api.Contracts.Student;
using School.Api.Contracts.User;
using School.Application.DTOs;
using School.Domain.Entitys;

namespace School.Api.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<StudentDto, StudentResponse>();
            CreateMap<LessonDto, LessonResponse>();
            CreateMap<UserDto, UserResponse>();
            CreateMap<AttendanceDayDto, AttendanceDayResponse>();
            CreateMap<AttendanceDelayDto, AttendanceDelayResponse>();
            CreateMap<AttendanceLogDto, AttendanceLogResponse>();
            CreateMap<StudentResponse, StudentDto>();
            CreateMap<LessonResponse, LessonDto>();
            CreateMap<UserResponse, UserDto>();
            CreateMap<AttendanceDayResponse, AttendanceDayDto>();
            CreateMap<AttendanceDelayResponse, AttendanceDelayDto>();
            CreateMap<AttendanceLogResponse, AttendanceLogDto>();
        }
    }
}
