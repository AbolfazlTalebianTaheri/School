namespace School.Api.Contracts.Attendance.AttendanceLog
{
    public record CreateAttendanceLogRequest(ushort studentId, int attendanceDayId, bool isAbsent);
}
