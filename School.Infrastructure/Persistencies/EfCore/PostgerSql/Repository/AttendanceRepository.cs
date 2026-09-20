using Microsoft.EntityFrameworkCore;
using School.Application.Repository;
using School.Domain.Entitys;
using School.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace School.Infrastructure.Persistencies.EfCore.PostgerSql.Repository
{
    public class AttendanceRepository(SchoolDbContext dbContext) : IAttendanceRepository
    {
        public async Task<IReadOnlyList<AttendanceLog>> GetAbsentsByDateAsync(DateOnly date, CancellationToken cancellationToken = default)
        {
            return await dbContext.AttendanceLogs
                .AsNoTracking()
                .Include(x => x.Student)
                .Where(x =>
                    x.AttendanceDay.Date == date &&
                    x.IsAbsent)
                .ToListAsync(cancellationToken);
        }
        public async Task AddLogAsync(AttendanceLog log, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(log);
            await dbContext.AttendanceLogs
                .AddAsync(log, cancellationToken);
        }
        public void UpdateLog(AttendanceLog log)
        {
            ArgumentNullException.ThrowIfNull(log);
            dbContext.AttendanceLogs.Update(log);
        }
        public void DeleteLog(AttendanceLog log)
        {
            ArgumentNullException.ThrowIfNull(log);
            dbContext.AttendanceLogs.Remove(log);
        }
        // ==================================================
        // AttendanceDelay
        // ==================================================
        public async Task<AttendanceDelay?> GetDelayByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            return await dbContext.AttendanceDelays
                .Include(x => x.AttendanceLog)
                    .ThenInclude(x => x.Student)
                .Include(x => x.Lesson)
                .FirstOrDefaultAsync(
                    x => x.Id == id,
                    cancellationToken);
        }
        public async Task<IReadOnlyList<AttendanceDelay>>
            GetDelaysByDateAsync(DateOnly date, CancellationToken cancellationToken = default)
        {
            return await dbContext.AttendanceDelays
                .AsNoTracking()
                .Include(x => x.Lesson)
                .Include(x => x.AttendanceLog)
                    .ThenInclude(x => x.Student)
                .Where(x =>
                    x.AttendanceLog.AttendanceDay.Date == date)
                .OrderBy(x => x.Class_Bell)
                .ToListAsync(cancellationToken);
        }
        public async Task<IReadOnlyList<AttendanceDelay>>
            GetStudentDelaysAsync(ushort studentId, DateOnly fromDate, DateOnly toDate, CancellationToken cancellationToken = default)
        {
            return await dbContext.AttendanceDelays
                .AsNoTracking()
                .Include(x => x.Lesson)
                .Include(x => x.AttendanceLog)
                    .ThenInclude(x => x.AttendanceDay)
                .Where(x =>
                    x.AttendanceLog.StudentId == studentId &&
                    x.AttendanceLog.AttendanceDay.Date >= fromDate &&
                    x.AttendanceLog.AttendanceDay.Date <= toDate)
                .OrderByDescending(
                    x => x.AttendanceLog.AttendanceDay.Date)
                .ToListAsync(cancellationToken);
        }
        public async Task<bool> DelayExistsAsync(int attendanceLogId, Class_Bell classBell, CancellationToken cancellationToken = default) =>
            await dbContext.AttendanceDelays
                .AnyAsync(
                    x =>
                        x.AttendanceLogId == attendanceLogId &&
                        x.Class_Bell == classBell,
                    cancellationToken);
        public async Task AddDelayAsync(AttendanceDelay delay, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(delay);
            await dbContext.AttendanceDelays
                .AddAsync(
                    delay,
                    cancellationToken);
        }
        public void UpdateDelay(AttendanceDelay delay)
        {
            ArgumentNullException.ThrowIfNull(delay);
            dbContext.AttendanceDelays.Update(delay);
        }
        public void DeleteDelay(AttendanceDelay delay)
        {
            ArgumentNullException.ThrowIfNull(delay);
            dbContext.AttendanceDelays.Remove(delay);
        }

        public Task<AttendanceDay?> GetDayByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<AttendanceDay?> GetDayByDateAsync(DateOnly date, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DayExistsAsync(DateOnly date, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task AddDayAsync(AttendanceDay day, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public void UpdateDay(AttendanceDay day)
        {
            throw new NotImplementedException();
        }

        public void DeleteDay(AttendanceDay day)
        {
            throw new NotImplementedException();
        }

        public Task<AttendanceLog?> GetLogByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<AttendanceLog?> GetStudentLogAsync(ushort studentId, DateOnly date, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<bool> StudentLogExistsAsync(ushort studentId, DateOnly date, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }
    }
}
