using School.Application.DTOs;

namespace School.Api.Contracts.Attendance.AttendanceLog
{
    public record AttendanceLogResponse(ushort studentId, StudentDto student, int attendanceDayId, AttendanceDayDto attendanceDay, bool isAbsent);
}