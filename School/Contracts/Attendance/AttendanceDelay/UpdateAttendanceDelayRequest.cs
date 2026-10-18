using School.Domain.Enums;

namespace School.Api.Contracts.Attendance.AttendanceDelay
{
    public record UpdateAttendanceDelayRequest(int attendanceLogId, Class_Bell class_Bell, ushort lessonId, int delayMinutes);
}
