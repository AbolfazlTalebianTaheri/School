using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualBasic;
using School.Api.Constants;
using School.Api.Contracts;
using School.Api.Contracts.Attendance;
using School.Api.Contracts.Attendance.AttendanceDay;
using School.Api.Contracts.Attendance.AttendanceDelay;
using School.Api.Contracts.Attendance.AttendanceLog;
using School.Application.DTOs;
using School.Application.Repository;
using School.Application.Service;
using School.Domain.Entitys;
using School.Domain.Enums;
using System.Threading;
using System.Threading.Tasks;
namespace School.Api.Controllers.V1
{
    public class AttendanceController(AttendanceService attendanceService, IMapper mapper) : BaseController
    {
        //[HttpPost(AttendanceUriContracts.AddDay)]
        //public async Task AddDayAsync(CreateAttendanceDayRequest day, CancellationToken cancellationToken = default) =>
        //    await attendanceService.AddDayAsync(mapper.Map<AttendanceDayDto>(day), cancellationToken);
        [Authorize(Roles = "Admin,Teacher")]
        [HttpPost(AttendanceUriContracts.SaveDay)]
        public async Task<ApiResult> SaveDay(DateOnly date, SaveAttendanceRequest request, CancellationToken cancellationToken = default)
        {
            var requestItems = request.Items ?? [];
            var invalidDelay = requestItems.Any(i => i.Delay is not null &&
                (i.IsAbsent || i.Delay.DelayMinutes < 1 || i.Delay.DelayMinutes > 240 || !Enum.IsDefined(i.Delay.ClassBell)));
            if (invalidDelay)
                return ApiResult.Failed(new { message = "اطلاعات تاخیر نامعتبر است." }, StatusCodes.Status400BadRequest);
            var items = requestItems.Select(i => new StudentAttendanceItem(
                i.StudentId,
                i.IsAbsent,
                i.Delay is null ? null : new StudentDelayInfo(i.Delay.ClassBell, i.Delay.LessonId, i.Delay.DelayMinutes))).ToList();
            await attendanceService.SaveDayAsync(date, items, cancellationToken);
            return ApiResult.NoContent();
        }
        [Authorize(Roles = "Admin,Teacher")]
        [HttpPost(AttendanceUriContracts.AddDelay)]
        public async Task AddDelayAsync(AttendanceDelayResponse delay, CancellationToken cancellationToken = default) =>
            await attendanceService.AddDelayAsync(mapper.Map<AttendanceDelayDto>(delay), cancellationToken);
        [Authorize(Roles = "Admin,Teacher")]
        [HttpPost(AttendanceUriContracts.AddLog)]
        public async Task AddLogAsync(CreateAttendanceLogRequest log, DateOnly date, CancellationToken cancellationToken = default) => await attendanceService.AddLogAsync(mapper.Map<AttendanceLogDto>(log), date, cancellationToken);
        //[HttpGet(AttendanceUriContracts.DayExists)]
        //public async Task<bool> DayExistsAsync(DateOnly date, CancellationToken cancellationToken = default) => await attendanceService.DayExistsAsync(date, cancellationToken);
        [Authorize(Roles = "Admin,Teacher")]
        [HttpGet(AttendanceUriContracts.DelayExists)]
        public async Task<bool> DelayExistsAsync(int attendanceLogId, Class_Bell classBell, CancellationToken cancellationToken = default) => await attendanceService.DelayExistsAsync(attendanceLogId, classBell, cancellationToken);
        [Authorize(Roles = "Admin,Teacher")]
        [HttpDelete(AttendanceUriContracts.DeleteDay)]
        public async Task DeleteDay(AttendanceDayResponse day) => await attendanceService.DeleteDayAsync(mapper.Map<AttendanceDayDto>(day));
        [Authorize(Roles = "Admin,Teacher")]
        [HttpDelete(AttendanceUriContracts.DeleteDelay)]
        public void DeleteDelay(AttendanceDelayResponse delay) => attendanceService.DeleteDelay(mapper.Map<AttendanceDelayDto>(delay));
        [Authorize(Roles = "Admin,Teacher")]
        [HttpDelete(AttendanceUriContracts.DeleteLog)]
        public async Task DeleteLog(UpdateAttendanceLogRequest log) =>await attendanceService.DeleteLogAsync(mapper.Map<AttendanceLogDto>(log));

        [HttpGet(AttendanceUriContracts.GetAbsentsByDate)]
        public async Task<IReadOnlyList<AttendanceLogResponse>> GetAbsentsByDateAsync(DateOnly date, CancellationToken cancellationToken = default)
        {
            var absents = mapper.Map<IReadOnlyList<AttendanceLogResponse>>(await attendanceService.GetAbsentsByDateAsync(date, cancellationToken));
            if (!IsParent)
                return absents;

            var own = OwnStudentIds; // والدین فقط غیبت دانش‌آموز خودشان را می‌بینند
            return absents.Where(l => own.Contains(l.studentId)).ToList();
        }
        [HttpGet(AttendanceUriContracts.GetDayByDate)]
        public async Task<AttendanceDayResponse?> GetDayByDateAsync(DateOnly date, CancellationToken cancellationToken = default) => mapper.Map<AttendanceDayResponse>(await attendanceService.GetDayByDateAsync(date, cancellationToken));
        [Authorize(Roles = "Admin,Teacher")]
        [HttpGet(AttendanceUriContracts.GetDayById)]
        public async Task<AttendanceDayResponse?> GetDayByIdAsync(int id, CancellationToken cancellationToken = default) => mapper.Map<AttendanceDayResponse>(await attendanceService.GetDayByIdAsync(id, cancellationToken));
        [Authorize(Roles = "Admin,Teacher")]
        [HttpGet(AttendanceUriContracts.GetDelayById)]
        public async Task<AttendanceDelayResponse?> GetDelayByIdAsync(int id, CancellationToken cancellationToken = default) => mapper.Map<AttendanceDelayResponse?>(await attendanceService.GetDelayByIdAsync(id, cancellationToken));
        [HttpGet(AttendanceUriContracts.GetDelaysByDate)]
        public async Task<IReadOnlyList<AttendanceDelayResponse>> GetDelaysByDateAsync(DateOnly date, CancellationToken cancellationToken = default)
        {
            var delays = mapper.Map<IReadOnlyList<AttendanceDelayResponse>>(await attendanceService.GetDelaysByDateAsync(date, cancellationToken));
            if (!IsParent)
                return delays;

            var own = OwnStudentIds; // والدین فقط تاخیر دانش‌آموز خودشان را می‌بینند
            return delays.Where(d => d.AttendanceLog is not null && own.Contains(d.AttendanceLog.studentId)).ToList();
        }
        [Authorize(Roles = "Admin,Teacher")]
        [HttpGet(AttendanceUriContracts.GetLogById)]
        public async Task<AttendanceLogResponse?> GetLogByIdAsync(int id, DateOnly date, CancellationToken cancellationToken = default) =>
            mapper.Map<AttendanceLogResponse?>(await attendanceService.GetLogByIdAsync(id, date, cancellationToken)); // کار نمی کند

        [HttpGet(AttendanceUriContracts.GetStudentDelays)]
        public async Task<IReadOnlyList<AttendanceDelayResponse>> GetStudentDelaysAsync(ushort studentId, DateOnly fromDate, DateOnly toDate, CancellationToken cancellationToken = default)
        {
            if (IsParent && !OwnStudentIds.Contains(studentId))
                return [];
            return mapper.Map<IReadOnlyList<AttendanceDelayResponse>>(await attendanceService.GetStudentDelaysAsync(studentId, fromDate, toDate, cancellationToken));
        }

        [HttpGet(AttendanceUriContracts.GetStudentLog)]
        public async Task<AttendanceLogResponse?> GetStudentLogAsync(ushort studentId, DateOnly date, CancellationToken cancellationToken = default)
        {
            if (IsParent && !OwnStudentIds.Contains(studentId))
                return null;
            return mapper.Map<AttendanceLogResponse?>(await attendanceService.GetStudentLogAsync(studentId, date, cancellationToken));
        }

        [Authorize(Roles = "Admin,Teacher")]
        [HttpGet(AttendanceUriContracts.StudentLogExists)]
        public async Task<bool> StudentLogExistsAsync(ushort studentId, DateOnly date, CancellationToken cancellationToken = default) => await attendanceService.StudentLogExistsAsync(studentId, date, cancellationToken);
        [Authorize(Roles = "Admin,Teacher")]
        [HttpPut(AttendanceUriContracts.UpdateDay)]
        public async Task UpdateDay(UpdateAttendanceDayRequest day) =>await attendanceService.UpdateDayAsync(mapper.Map<AttendanceDayDto>(day));

        [Authorize(Roles = "Admin,Teacher")]
        [HttpPut(AttendanceUriContracts.UpdateDelay)]
        public void UpdateDelay(AttendanceDelayResponse delay) => attendanceService.UpdateDelay(mapper.Map<AttendanceDelayDto>(delay));
        [Authorize(Roles = "Admin,Teacher")]
        [HttpPut(AttendanceUriContracts.UpdateLog)]
        public void UpdateLog(UpdateAttendanceLogRequest log) => attendanceService.UpdateLog(mapper.Map<AttendanceLogDto>(log));
    }
}
