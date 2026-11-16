using AutoMapper;
using School.Application.DTOs;
using School.Application.DTOs.User;
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
            CreateMap<User, UserRsponseDto>()
                .ForMember(d => d.NationalCode, o => o.MapFrom(s => s.Students.Select(x => x.NationalCode).FirstOrDefault()));
            CreateMap<AttendanceLog, AttendanceLogDto>();
            CreateMap<Lesson, LessonDto>();
            CreateMap<AttendanceDay, AttendanceDayDto>();
            CreateMap<AttendanceDelay,AttendanceDelayDto>();
            CreateMap<AttendanceLog,AttendanceLogDto>();
            CreateMap<StudentDto, Student>();
            CreateMap<UserRsponseDto, User>().ForMember(x=>x.PassWordHash,y=>y.MapFrom(s=>s.Password));
            CreateMap<AttendanceLogDto, AttendanceLog>();
            CreateMap<LessonDto, Lesson>();
            CreateMap<AttendanceDayDto, AttendanceDay>();
            CreateMap<AttendanceDelayDto, AttendanceDelay>();
            CreateMap<AttendanceLogDto, AttendanceLog>();
        }
    }
}
