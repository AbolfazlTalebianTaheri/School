using AutoMapper;
using School.Application.DTOs;
using School.Domain.Entitys;
using System;
using System.Collections.Generic;
using System.Text;

namespace School.Application.Mapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Student,StudentDto>();
            CreateMap<User, UserDto>();
            CreateMap<AttendanceLog, AttendanceLogDto>();
            CreateMap<Lesson, LessonDto>();
            CreateMap<AttendanceDay, AttendanceDayDto>();
            CreateMap<AttendanceDelay,AttendanceDelayDto>();
            CreateMap<AttendanceLog,AttendanceLogDto>();
            CreateMap<StudentDto, Student>();
            CreateMap<UserDto, User>();
            CreateMap<AttendanceLogDto, AttendanceLog>();
            CreateMap<LessonDto, Lesson>();
            CreateMap<AttendanceDayDto, AttendanceDay>();
            CreateMap<AttendanceDelayDto, AttendanceDelay>();
            CreateMap<AttendanceLogDto, AttendanceLog>();
        }
    }
}
