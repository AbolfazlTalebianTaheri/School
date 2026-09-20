namespace School.Api.Contracts.Attendance.AttendanceLog
{
    public record AttendanceLogResponse(ushort studentId, int attendanceDayId, bool isAbsent);
}
