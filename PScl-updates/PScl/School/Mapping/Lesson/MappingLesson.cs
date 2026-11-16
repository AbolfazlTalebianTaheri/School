using AutoMapper;
using School.Api.Contracts.Lesson;
using School.Application.DTOs;
using School.Domain.Entitys;

namespace School.Api.Mapping.User
{
    public class MappingLesson :Profile
    {
        public MappingLesson()
        {
            CreateMap<LessonResponse, LessonDto>();
            CreateMap<CreateLessonRequest, LessonDto>();
            CreateMap<UpdateLessonRequest, LessonDto>();

            CreateMap<LessonDto, LessonResponse>();
            CreateMap<Lesson, LessonResponse>();

        }
    }
}
