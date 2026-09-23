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
        public async Task<bool> DayExistsAsync(DateOnly date, CancellationToken cancellationToken = default) => await attendanceRepository.DayExistsAsync(date, cancellationToken);
        public async Task AddDayAsync(AttendanceDayDto day, CancellationToken cancellationToken = default)
        {
            await attendanceRepository.AddDayAsync(mapper.Map<AttendanceDay>(day), cancellationToken);
            await SaveChangeAsync(cancellationToken);
        }
        public async void UpdateDay(AttendanceDayDto day)
        {
            if (await DayExistsAsync(day.Date))
                attendanceRepository.UpdateDay(mapper.Map<AttendanceDay>(day));
        }
        public async void DeleteDay(AttendanceDayDto day)
        {
            if (await DayExistsAsync(day.Date))
                attendanceRepository.DeleteDay(mapper.Map<AttendanceDay>(day));
        }
        public async Task<AttendanceLogDto?> GetLogByIdAsync(int id,DateOnly date, CancellationToken cancellationToken = default) => mapper.Map<AttendanceLogDto>(await attendanceRepository.GetLogByIdAsync(id,date, cancellationToken));
        public async Task<AttendanceLogDto?> GetStudentLogAsync(ushort studentId, DateOnly date, CancellationToken cancellationToken = default) => mapper.Map<AttendanceLogDto>(await attendanceRepository.GetStudentLogAsync(studentId, date, cancellationToken));
        public async Task<bool> StudentLogExistsAsync(ushort studentId, DateOnly date, CancellationToken cancellationToken = default) => await attendanceRepository.StudentLogExistsAsync(studentId, date, cancellationToken);
        public async Task<IReadOnlyList<AttendanceLogDto>> GetAbsentsByDateAsync(DateOnly date, CancellationToken cancellationToken = default) => mapper.Map<IReadOnlyList<AttendanceLogDto>>(attendanceRepository);
        public async Task AddLogAsync(AttendanceLogDto log,DateOnly date, CancellationToken cancellationToken = default)
        {
            var isExists = await DayExistsAsync(date, cancellationToken);
            if (!isExists)
                await AddDayAsync(new AttendanceDayDto()
                {
                    Date = date
                }, cancellationToken);
                await attendanceRepository.AddLogAsync(mapper.Map<AttendanceLog>(log), date ,cancellationToken);
            await SaveChangeAsync(cancellationToken);
        }
        public async void UpdateLog(AttendanceLogDto log) => attendanceRepository.UpdateLog(mapper.Map<AttendanceLog>(log));
        public async void DeleteLog(AttendanceLogDto log) => attendanceRepository.DeleteLog(mapper.Map<AttendanceLog>(log));
        public async Task<AttendanceDelayDto?> GetDelayByIdAsync(int id, CancellationToken cancellationToken = default) => mapper.Map<AttendanceDelayDto?>(await attendanceRepository.GetDelayByIdAsync(id, cancellationToken));
        public async Task<IReadOnlyList<AttendanceDelayDto>> GetDelaysByDateAsync(DateOnly date, CancellationToken cancellationToken = default) => mapper.Map<IReadOnlyList<AttendanceDelayDto>>(await attendanceRepository.GetDelaysByDateAsync(date, cancellationToken));
        public async Task<IReadOnlyList<AttendanceDelayDto>> GetStudentDelaysAsync(ushort studentId, DateOnly fromDate, DateOnly toDate, CancellationToken cancellationToken = default) =>
            mapper.Map<IReadOnlyList<AttendanceDelayDto>>(await attendanceRepository.GetStudentDelaysAsync(studentId, fromDate, toDate, cancellationToken));
        public async Task<bool> DelayExistsAsync(int attendanceLogId, Class_Bell classBell, CancellationToken cancellationToken = default) => await attendanceRepository.DelayExistsAsync(attendanceLogId, classBell, cancellationToken);
        public async Task AddDelayAsync(AttendanceDelayDto delay, CancellationToken cancellationToken = default) => await attendanceRepository.AddDelayAsync(mapper.Map<AttendanceDelay>(delay), cancellationToken);
        public async void UpdateDelay(AttendanceDelayDto delay) => attendanceRepository.UpdateDelay(mapper.Map<AttendanceDelay>(delay));
        public async void DeleteDelay(AttendanceDelayDto delay) => attendanceRepository.DeleteDelay(mapper.Map<AttendanceDelay>(delay));
        private async Task SaveChangeAsync(CancellationToken cancellationToken = default) => await attendanceRepository.SaveChangeAsync(cancellationToken);
    }
}