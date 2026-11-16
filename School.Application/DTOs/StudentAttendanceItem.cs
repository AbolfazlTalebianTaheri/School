using School.Domain.Enums;

namespace School.Application.DTOs
{
    public record StudentDelayInfo(Class_Bell ClassBell, ushort LessonId, int DelayMinutes);

    public record StudentAttendanceItem(ushort StudentId, bool IsAbsent, StudentDelayInfo? Delay = null);
}
