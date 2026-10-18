using School.Domain.Enums;

namespace School.Api.Contracts.Attendance.AttendanceDelay
{
    public record CreateAttendanceDelayRequest(int attendanceLogId, Class_Bell class_Bell, ushort lessonId, int delayMinutes);
}
