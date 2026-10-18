using School.Api.Contracts.Attendance.AttendanceLog;
using School.Api.Contracts.Lesson;
using School.Domain.Enums;

namespace School.Api.Contracts.Attendance.AttendanceDelay
{
    public record AttendanceDelayResponse(int AttendanceLogId, AttendanceLogResponse AttendanceLog,
        Class_Bell Class_Bell, short LessonId, LessonResponse Lesson, int DelayMinutes
);
}
