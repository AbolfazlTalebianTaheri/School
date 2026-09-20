using School.Domain.Entitys;
using School.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace School.Application.Repository
{
    public interface IAttendanceRepository
    {
        // ========================
        // AttendanceDay
        // ========================
        Task<AttendanceDay?> GetDayByIdAsync(
            int id,
            CancellationToken cancellationToken = default);
        Task<AttendanceDay?> GetDayByDateAsync(
            DateOnly date,
            CancellationToken cancellationToken = default);
        Task<bool> DayExistsAsync(
            DateOnly date,
            CancellationToken cancellationToken = default);
        Task AddDayAsync(
            AttendanceDay day,
            CancellationToken cancellationToken = default);
        void UpdateDay(AttendanceDay day);
        void DeleteDay(AttendanceDay day);
        // ========================
        // AttendanceLog
        // ========================
        Task<AttendanceLog?> GetLogByIdAsync(
            int id,
            CancellationToken cancellationToken = default);
        Task<AttendanceLog?> GetStudentLogAsync(
            ushort studentId,
            DateOnly date,
            CancellationToken cancellationToken = default);
        Task<bool> StudentLogExistsAsync(
            ushort studentId,
            DateOnly date,
            CancellationToken cancellationToken = default);
        Task<IReadOnlyList<AttendanceLog>> GetAbsentsByDateAsync(
            DateOnly date,
            CancellationToken cancellationToken = default);
        Task AddLogAsync(
            AttendanceLog log,
            CancellationToken cancellationToken = default);
        void UpdateLog(AttendanceLog log);
        void DeleteLog(AttendanceLog log);
        // ========================
        // AttendanceDelay
        // ========================
        Task<AttendanceDelay?> GetDelayByIdAsync(
            int id,
            CancellationToken cancellationToken = default);
        Task<IReadOnlyList<AttendanceDelay>> GetDelaysByDateAsync(
            DateOnly date,
            CancellationToken cancellationToken = default);
        Task<IReadOnlyList<AttendanceDelay>> GetStudentDelaysAsync(
            ushort studentId,
            DateOnly fromDate,
            DateOnly toDate,
            CancellationToken cancellationToken = default);
        Task<bool> DelayExistsAsync(
            int attendanceLogId,
            Class_Bell classBell,
            CancellationToken cancellationToken = default);
        Task AddDelayAsync(
            AttendanceDelay delay,
            CancellationToken cancellationToken = default);
        void UpdateDelay(AttendanceDelay delay);
        void DeleteDelay(AttendanceDelay delay);
    }
}
