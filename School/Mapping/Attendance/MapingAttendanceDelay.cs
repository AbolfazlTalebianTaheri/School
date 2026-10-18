using AutoMapper;
using School.Api.Contracts.Attendance.AttendanceDay;
using School.Api.Contracts.Attendance.AttendanceDelay;
using School.Api.Contracts.Attendance.AttendanceLog;
using School.Api.Contracts.Lesson;
using School.Application.DTOs;
using School.Domain.Entitys;

namespace School.Api.Mapping.Attendance
{
    public class MapingAttendanceDelay : Profile
    {
        public MapingAttendanceDelay()
        {
            //CreateMap<AttendanceDayDto, AttendanceDayResponse>();
            //CreateMap<AttendanceDelayDto, AttendanceDelayResponse>();
            //CreateMap<AttendanceLogDto, AttendanceLogResponse>();
            //CreateMap<AttendanceDayResponse, AttendanceDayDto>();
            //CreateMap<AttendanceDelayResponse, AttendanceDelayDto>();
            //CreateMap<AttendanceLogResponse, AttendanceLogDto>();

            // DTO <-> Response
            CreateMap<AttendanceDayDto, AttendanceDayResponse>();
            CreateMap<AttendanceDelayDto, AttendanceDelayResponse>();
            CreateMap<AttendanceLogDto, AttendanceLogResponse>();

            CreateMap<AttendanceDayResponse, AttendanceDayDto>();
            CreateMap<AttendanceDelayResponse, AttendanceDelayDto>();
            CreateMap<AttendanceLogResponse, AttendanceLogDto>();

            // Entity -> Response
            CreateMap<AttendanceLog, AttendanceLogResponse>();
            CreateMap<Lesson, LessonResponse>();

            // DTO <-> Lesson Response
            CreateMap<LessonDto, LessonResponse>();
            CreateMap<LessonResponse, LessonDto>();
        }
    }
}
