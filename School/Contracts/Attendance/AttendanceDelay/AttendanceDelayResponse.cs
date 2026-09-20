using School.Domain.Enums;

namespace School.Api.Contracts.Attendance.AttendanceDelay
{
    public record AttendanceDelayResponse(int attendanceLogId, Class_Bell class_Bell, int lessonId, int delayMinutes);
}
