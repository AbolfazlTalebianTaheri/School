using AutoMapper;
using School.Api.Contracts.Attendance.AttendanceLog;
using School.Application.DTOs;

namespace School.Api.Mapping.Attendance
{
    public class MapingAttendanceLog : Profile
    {
        public MapingAttendanceLog()
        {
            CreateMap<CreateAttendanceLogRequest,AttendanceLogDto>();
            CreateMap<AttendanceLogDto, CreateAttendanceLogRequest>();

            CreateMap<AttendanceLogDto, UpdateAttendanceLogRequest>();
            CreateMap<UpdateAttendanceLogRequest, AttendanceLogDto>();
            
            CreateMap<AttendanceLogResponse, AttendanceLogDto>();
            CreateMap<AttendanceLogDto, AttendanceLogResponse>();
        }
    }
}
