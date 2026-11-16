using AutoMapper;
using School.Application.DTOs;
using School.Application.Repository;
using School.Domain.Entitys;
using School.Domain.Enums;
using School.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Text;

namespace School.Application.Service
{
    public class AttendanceService(IAttendanceRepository attendanceRepository, IMapper mapper)
    {
        public async Task<AttendanceDayDto?> GetDayByIdAsync(int id, CancellationToken cancellationToken = default) => mapper.Map<AttendanceDayDto?>(await attendanceRepository.GetDayByIdAsync(id, cancellationToken));
        public async Task<AttendanceDayDto?> GetDayByDateAsync(DateOnly date, CancellationToken cancellationToken = default) => mapper.Map<AttendanceDayDto>(await attendanceRepository.GetDayByDateAsync(date, cancellationToken));
        private async Task<bool> DayExistsAsync(DateOnly date, CancellationToken cancellationToken = default) => await attendanceRepository.DayExistsAsync(date, cancellationToken);
        private async Task<AttendanceDay?> AddDayAsync(AttendanceDayDto day, CancellationToken cancellationToken = default)
        {
            var response = await attendanceRepository.AddDayAsync(mapper.Map<AttendanceDay>(day), cancellationToken);
            await SaveChangeAsync(cancellationToken);
            return response;
        }
        public async Task UpdateDayAsync(AttendanceDayDto day)
        {
            if (await DayExistsAsync(day.Date))
                attendanceRepository.UpdateDay(mapper.Map<AttendanceDay>(day));
        }
        public async Task DeleteDayAsync(AttendanceDayDto day)
        {
            if (await DayExistsAsync(day.Date))
            {
                await attendanceRepository.DeleteDay(mapper.Map<AttendanceDay>(day));
                await SaveChangeAsync();
            }
        }
        public async Task<AttendanceLogDto?> GetLogByIdAsync(int id, DateOnly date, CancellationToken cancellationToken = default) => mapper.Map<AttendanceLogDto>(await attendanceRepository.GetLogByIdAsync(id, date, cancellationToken));
        public async Task<AttendanceLogDto?> GetStudentLogAsync(ushort studentId, DateOnly date, CancellationToken cancellationToken = default) => mapper.Map<AttendanceLogDto>(await attendanceRepository.GetStudentLogAsync(studentId, date, cancellationToken));
        public async Task<bool> StudentLogExistsAsync(ushort studentId, DateOnly date, CancellationToken cancellationToken = default) => await attendanceRepository.StudentLogExistsAsync(studentId, date, cancellationToken);
        public async Task<IReadOnlyList<AttendanceLogDto>> GetAbsentsByDateAsync(DateOnly date, CancellationToken cancellationToken = default) => mapper.Map<IReadOnlyList<AttendanceLogDto>>(await attendanceRepository.GetAbsentsByDateAsync(date,cancellationToken));
        public async Task AddLogAsync(AttendanceLogDto log, DateOnly date, CancellationToken cancellationToken = default)
        {
            var isExists = await DayExistsAsync(date, cancellationToken);
            if (!isExists)
            {
                await AddDayAsync(new AttendanceDayDto()
                {
                    Date = date
                }, cancellationToken);
            }
            log.Id = 0;
            var existsLogStudent =await StudentLogExistsAsync(log.StudentId,date, cancellationToken);
            if (existsLogStudent)
                throw new DuplicateLogForStudentInDateException();
            await attendanceRepository.AddLogAsync(mapper.Map<AttendanceLog>(log), date, cancellationToken);
            await SaveChangeAsync(cancellationToken);
        }
        /// <summary>
        /// ثبت حضور/غیاب چند دانش‌آموز برای یک روز (اگر روز یا لاگ وجود نداشته باشد ساخته می‌شود، وگرنه بروزرسانی می‌شود)
        /// </summary>
        public async Task SaveDayAsync(DateOnly date, IReadOnlyList<StudentAttendanceItem> items, CancellationToken cancellationToken = default)
        {
            var day = await attendanceRepository.GetDayWithLogsByDateAsync(date, cancellationToken);
            if (day is null)
            {
                day = new AttendanceDay(date);
                await attendanceRepository.AddDayAsync(day, cancellationToken);
                await attendanceRepository.SaveChangeAsync(cancellationToken);
            }
            foreach (var item in items.GroupBy(x => x.StudentId).Select(g => g.Last()))
            {
                var log = day.Logs.FirstOrDefault(x => x.StudentId == item.StudentId);
                if (log is null)
                {
                    log = new AttendanceLog(item.StudentId);
                    day.AddLog(log);
                }
                if (item.IsAbsent)
                    log.MarkAbsent();
                else if (item.Delay is not null)
                    log.SetDelay(item.Delay.ClassBell, item.Delay.LessonId, item.Delay.DelayMinutes);
                else
                    log.MarkPresent();
            }
            await SaveChangeAsync(cancellationToken);
        }
        public void UpdateLog(AttendanceLogDto log) => attendanceRepository.UpdateLog(mapper.Map<AttendanceLog>(log));
        public async Task DeleteLogAsync(AttendanceLogDto log, CancellationToken cancellationToken = default)
        {
            var date = await attendanceRepository.GetDayByIdAsync(log.AttendanceDayId, cancellationToken);
            if (date is null)
                return;
            var logStudentInDate = await attendanceRepository.GetStudentLogAsync(log.StudentId, date.Date, cancellationToken);
            if (logStudentInDate is null) return;
            attendanceRepository.DeleteLog(logStudentInDate);
        }
        public async Task<AttendanceDelayDto?> GetDelayByIdAsync(int id, CancellationToken cancellationToken = default) => mapper.Map<AttendanceDelayDto?>(await attendanceRepository.GetDelayByIdAsync(id, cancellationToken));
        public async Task<IReadOnlyList<AttendanceDelayDto>> GetDelaysByDateAsync(DateOnly date, CancellationToken cancellationToken = default) => mapper.Map<IReadOnlyList<AttendanceDelayDto>>(await attendanceRepository.GetDelaysByDateAsync(date, cancellationToken));
        public async Task<IReadOnlyList<AttendanceDelayDto>> GetStudentDelaysAsync(ushort studentId, DateOnly fromDate, DateOnly toDate, CancellationToken cancellationToken = default) =>
            mapper.Map<IReadOnlyList<AttendanceDelayDto>>(await attendanceRepository.GetStudentDelaysAsync(studentId, fromDate, toDate, cancellationToken));
        public async Task<bool> DelayExistsAsync(int attendanceLogId, Class_Bell classBell, CancellationToken cancellationToken = default) => await attendanceRepository.DelayExistsAsync(attendanceLogId, classBell, cancellationToken);
        public async Task AddDelayAsync(AttendanceDelayDto delay, CancellationToken cancellationToken = default)
        {
            await attendanceRepository.AddDelayAsync(mapper.Map<AttendanceDelay>(delay), cancellationToken);
            await SaveChangeAsync();

        }
        public void UpdateDelay(AttendanceDelayDto delay) => attendanceRepository.UpdateDelay(mapper.Map<AttendanceDelay>(delay));
        public void DeleteDelay(AttendanceDelayDto delay) => attendanceRepository.DeleteDelay(mapper.Map<AttendanceDelay>(delay));
        private async Task SaveChangeAsync(CancellationToken cancellationToken = default) => await attendanceRepository.SaveChangeAsync(cancellationToken);
    }
}