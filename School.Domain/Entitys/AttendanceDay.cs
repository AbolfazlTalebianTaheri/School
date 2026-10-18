using School.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Runtime;
using System.Text;

namespace School.Domain.Entitys
{
    public class AttendanceDay : BaseClass<int>
    {
        private readonly List<AttendanceLog> _logs = new();

        public DateOnly Date { get; private set; }

        public IReadOnlyCollection<AttendanceLog> Logs => _logs;

        private AttendanceDay() { }
        public AttendanceDay(int id,DateOnly date)
        {
            Id = id;
            Date = date;
        }

        public AttendanceDay(DateOnly date) =>
            Date = date;
        public void AddLog(AttendanceLog log)
        {
            ArgumentNullException.ThrowIfNull(log); // check if null exception or next step
            var alreadyExists = _logs.Any(x => x.StudentId == log.StudentId);
            if (alreadyExists)
                throw new DuplicateUserNameException();
            _logs.Add(log);
        }
    }
}
