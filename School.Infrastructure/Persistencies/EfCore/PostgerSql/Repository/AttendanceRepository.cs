using Microsoft.EntityFrameworkCore;
using School.Application.Repository;
using School.Domain.Entitys;
using School.Domain.Enums;
using School.Domain.InterFaces;
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
        public async Task AddLogAsync(ushort studentId, DateOnly date, CancellationToken cancellationToken = default)
        {
            var day = await dbContext.AttendanceDays.Include(x => x.Logs)
                .FirstOrDefaultAsync(x => x.Date == date);
            if (day is null)
                return;
            var logStudent = new AttendanceLog(studentId);
            day.AddLog(logStudent);
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
            var log = await dbContext.AttendanceLogs.FirstOrDefaultAsync(x => x.Id == delay.AttendanceLogId);
            if (log is null)
                return;
            log.AddDelay(delay.Class_Bell, delay.LessonId, delay.DelayMinutes);
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

        public async Task<AttendanceDay?> GetDayByIdAsync(int id, CancellationToken cancellationToken = default) => await dbContext.AttendanceDays.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
        public async Task<AttendanceDay?> GetDayByDateAsync(DateOnly date, CancellationToken cancellationToken = default) => await dbContext.AttendanceDays.FirstOrDefaultAsync(x => x.Date == date, cancellationToken);
        public async Task<bool> DayExistsAsync(DateOnly date, CancellationToken cancellationToken = default) => await dbContext.AttendanceDays.AnyAsync(x => x.Date == date);

        public async Task<AttendanceDay?> AddDayAsync(AttendanceDay day, CancellationToken cancellationToken = default)
        {
            await dbContext.AttendanceDays.AddAsync(day, cancellationToken);
            var attendanceDay = await dbContext.AttendanceDays.FirstOrDefaultAsync(x => x.Date == day.Date);
            if (attendanceDay is null)
                return null;
            return new AttendanceDay(attendanceDay.Id, attendanceDay.Date);
        }
        public void UpdateDay(AttendanceDay day) => dbContext.Update(day);


        public async Task DeleteDay(AttendanceDay day)
        {
            var dayDelete = await dbContext.AttendanceDays.FirstOrDefaultAsync(x => x.Date == day.Date);
            if (dayDelete is null)
                return;
            dayDelete.IsDeleted = true;
        }


        public async Task<AttendanceLog?> GetLogByIdAsync(int id, DateOnly date, CancellationToken cancellationToken = default)
        {
            var dayLog = await dbContext.AttendanceDays.Include(x => x.Logs)
                .FirstOrDefaultAsync(x => x.Date == date, cancellationToken);
            if (dayLog is null) return null;
            return dayLog.Logs.FirstOrDefault(x => x.Id == id);
        }

        public async Task<AttendanceLog?> GetStudentLogAsync(ushort studentId, DateOnly date, CancellationToken cancellationToken = default)
        {
            var dayLog = await dbContext.AttendanceDays.Include(x => x.Logs)
                .FirstOrDefaultAsync(x => x.Date == date, cancellationToken);
            if (dayLog is null) return null;
            return dayLog.Logs.FirstOrDefault(x => x.StudentId == studentId);
        }

        public async Task<bool> StudentLogExistsAsync(ushort studentId, DateOnly date, CancellationToken cancellationToken = default)
        {
            var daylog = await dbContext.AttendanceDays.Include(x => x.Logs)
                .FirstOrDefaultAsync(x => x.Date == date, cancellationToken);
            //if (daylog is null) return false;
            return daylog.Logs.Any(x => x.StudentId == studentId);
        }
        public async Task SaveChangeAsync(CancellationToken cancellationToken = default) => await dbContext.SaveChangesAsync(cancellationToken);

        public async Task AddLogAsync(AttendanceLog log, DateOnly date, CancellationToken cancellationToken = default)
        {
            var dayLog = await dbContext.AttendanceDays.Include(x => x.Logs)
                .FirstOrDefaultAsync(x => x.Date == date, cancellationToken);
            if (dayLog is null) return;
            dayLog.AddLog(log);
        }
    }
}
