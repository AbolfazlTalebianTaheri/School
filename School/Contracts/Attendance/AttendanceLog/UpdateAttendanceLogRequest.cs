namespace School.Api.Contracts.Attendance.AttendanceLog
{
    public record UpdateAttendanceLogRequest(int id,ushort studentId, int attendanceDayId, bool isAbsent);
}
