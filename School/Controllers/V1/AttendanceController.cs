using AutoMapper;
using Microsoft.AspNetCore.Components.Forms.Mapping;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualBasic;
using School.Api.Constants;
using School.Api.Contracts.Attendance.AttendanceDay;
using School.Api.Contracts.Attendance.AttendanceDelay;
using School.Api.Contracts.Attendance.AttendanceLog;
using School.Application.DTOs;
using School.Application.Repository;
using School.Application.Service;
using School.Domain.Entitys;
using School.Domain.Enums;
using System.Threading;
namespace School.Api.Controllers.V1
{
    public class AttendanceController(AttendanceService attendanceService, IMapper mapper) : BaseController
    {
        [HttpPost(AttendanceUriContracts.AddDay)]
        public async Task AddDayAsync(AttendanceDayResponse day, CancellationToken cancellationToken = default) =>
            await attendanceService.AddDayAsync(mapper.Map<AttendanceDayDto>(day), cancellationToken);
        [HttpPost(AttendanceUriContracts.AddDay)]
        public async Task AddDelayAsync(AttendanceDelayResponse delay, CancellationToken cancellationToken = default) =>
            await attendanceService.AddDelayAsync(mapper.Map<AttendanceDelayDto>(delay), cancellationToken);
        [HttpPost(AttendanceUriContracts.AddLog)]
        public async Task AddLogAsync(AttendanceLogResponse log, CancellationToken cancellationToken = default) => await attendanceService.AddLogAsync(mapper.Map<AttendanceLogDto>(log), cancellationToken);
        [HttpGet(AttendanceUriContracts.DayExists)]
        public async Task<bool> DayExistsAsync(DateOnly date, CancellationToken cancellationToken = default) => await attendanceService.DayExistsAsync(date, cancellationToken);
        [HttpGet(AttendanceUriContracts.DelayExists)]
        public async Task<bool> DelayExistsAsync(int attendanceLogId, Class_Bell classBell, CancellationToken cancellationToken = default) => await attendanceService.DelayExistsAsync(attendanceLogId, classBell, cancellationToken);
        [HttpGet(AttendanceUriContracts.DeleteDay)]
        public void DeleteDay(AttendanceDayResponse day) => attendanceService.DeleteDay(mapper.Map<AttendanceDayDto>(day));
        [HttpGet(AttendanceUriContracts.DeleteDelay)]
        public void DeleteDelay(AttendanceDelayResponse delay) => attendanceService.DeleteDelay(mapper.Map<AttendanceDelayDto>(delay));
        [HttpGet(AttendanceUriContracts.DeleteLog)]
        public void DeleteLog(AttendanceLogResponse log) => attendanceService.DeleteLog(mapper.Map<AttendanceLogDto>(log));
        [HttpGet(AttendanceUriContracts.GetAbsentsByDate)]
        public async Task<IReadOnlyList<AttendanceLogResponse>> GetAbsentsByDateAsync(DateOnly date, CancellationToken cancellationToken = default) => mapper.Map<IReadOnlyList<AttendanceLogResponse>>(await attendanceService.GetAbsentsByDateAsync(date, cancellationToken));
        [HttpGet(AttendanceUriContracts.GetDayByDate)]
        public async Task<AttendanceDayResponse?> GetDayByDateAsync(DateOnly date, CancellationToken cancellationToken = default) => mapper.Map<AttendanceDayResponse>(await attendanceService.GetDayByDateAsync(date, cancellationToken));
        [HttpGet(AttendanceUriContracts.GetDayById)]
        public async Task<AttendanceDayResponse?> GetDayByIdAsync(int id, CancellationToken cancellationToken = default) => mapper.Map<AttendanceDayResponse>(await attendanceService.GetDayByIdAsync(id, cancellationToken));
        [HttpGet(AttendanceUriContracts.GetDelayById)]
        public async Task<AttendanceDelayResponse?> GetDelayByIdAsync(int id, CancellationToken cancellationToken = default) => mapper.Map<AttendanceDelayResponse?>(await attendanceService.GetDelayByIdAsync(id, cancellationToken));
        [HttpGet(AttendanceUriContracts.GetDelaysByDate)]
        public async Task<IReadOnlyList<AttendanceDelayResponse>> GetDelaysByDateAsync(DateOnly date, CancellationToken cancellationToken = default) => mapper.Map<IReadOnlyList<AttendanceDelayResponse>>(await attendanceService.GetDelaysByDateAsync(date, cancellationToken));
        [HttpGet(AttendanceUriContracts.GetLogById)]
        public async Task<AttendanceLogResponse?> GetLogByIdAsync(int id, CancellationToken cancellationToken = default) =>
            mapper.Map<AttendanceLogResponse?>(await attendanceService.GetLogByIdAsync(id, cancellationToken));
        [HttpGet(AttendanceUriContracts.GetStudentDelays)]
        public async Task<IReadOnlyList<AttendanceDelayResponse>> GetStudentDelaysAsync(ushort studentId, DateOnly fromDate, DateOnly toDate, CancellationToken cancellationToken = default) => mapper.Map<IReadOnlyList<AttendanceDelayResponse>>(await attendanceService.GetStudentDelaysAsync(studentId, fromDate, toDate, cancellationToken));
        [HttpGet(AttendanceUriContracts.GetStudentLog)]
        public async Task<AttendanceLogResponse?> GetStudentLogAsync(ushort studentId, DateOnly date, CancellationToken cancellationToken = default) => mapper.Map<AttendanceLogResponse?>(await attendanceService.GetStudentLogAsync(studentId, date, cancellationToken));
        [HttpGet(AttendanceUriContracts.StudentLogExists)]
        public async Task<bool> StudentLogExistsAsync(ushort studentId, DateOnly date, CancellationToken cancellationToken = default) => await attendanceService.StudentLogExistsAsync(studentId, date, cancellationToken); [HttpGet(AttendanceUriContracts.UpdateDay)]
        public void UpdateDay(AttendanceDayResponse day) => attendanceService.UpdateDay(mapper.Map<AttendanceDayDto>(day));
        [HttpGet(AttendanceUriContracts.UpdateDelay)]
        public void UpdateDelay(AttendanceDelayResponse delay) => attendanceService.UpdateDelay(mapper.Map<AttendanceDelayDto>(delay));
        [HttpGet(AttendanceUriContracts.UpdateLog)]
        public void UpdateLog(AttendanceLogResponse log) => attendanceService.UpdateLog(mapper.Map<AttendanceLogDto>(log));
    }
}
