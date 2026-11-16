using School.Domain.Enums;

namespace School.Api.Contracts.Attendance
{
    public record SaveAttendanceDelay(Class_Bell ClassBell, ushort LessonId, int DelayMinutes);

    public record SaveAttendanceItem(ushort StudentId, bool IsAbsent, SaveAttendanceDelay? Delay = null);

    public record SaveAttendanceRequest(List<SaveAttendanceItem> Items);
}
