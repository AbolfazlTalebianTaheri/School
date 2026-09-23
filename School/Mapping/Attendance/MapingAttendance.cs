using AutoMapper;
using School.Api.Contracts.Attendance.AttendanceDay;
using School.Api.Contracts.Attendance.AttendanceDelay;
using School.Api.Contracts.Attendance.AttendanceLog;
using School.Application.DTOs;

namespace School.Api.Mapping.Attendance
{
    public class MapingAttendance : Profile
    {
        public MapingAttendance()
        {
            CreateMap<AttendanceDayDto, AttendanceDayResponse>();
            CreateMap<AttendanceDelayDto, AttendanceDelayResponse>();
            CreateMap<AttendanceLogDto, AttendanceLogResponse>();
            CreateMap<AttendanceDayResponse, AttendanceDayDto>();
            CreateMap<AttendanceDelayResponse, AttendanceDelayDto>();
            CreateMap<AttendanceLogResponse, AttendanceLogDto>();
        }
    }
}
