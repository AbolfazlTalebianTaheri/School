using School.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace School.Domain.Entitys
{
    /// <summary>
    /// for Student if absence or Delay
    /// </summary>
    public class AttendanceLog : BaseClass<int>
    {
        private readonly List<AttendanceDelay> _delays = new();
        public ushort StudentId { get; private set; }
        public Student Student { get; private set; } = null!;
        public int AttendanceDayId { get; private set; }
        public AttendanceDay AttendanceDay { get; private set; } = null!;
        public bool IsAbsent { get; private set; } //  If absence has done or no
        public IReadOnlyCollection<AttendanceDelay> Delays => _delays.AsReadOnly();
        private AttendanceLog() { }
        public AttendanceLog(ushort studentId)
        {
            StudentId = studentId;
            IsAbsent = false;
        }
        public void MarkAbsent()
        {
            IsAbsent = true;
            _delays.Clear(); // An absent student is not considered late.
        }
        public void AddDelay(Class_Bell class_Bell, ushort lessonId,int delayMinutes)
        {
            if (IsAbsent)
                throw new InvalidOperationException("An absent student is not considered late.");
            var delay = new AttendanceDelay(class_Bell,lessonId,delayMinutes);
            _delays.Add(delay);
        }

    }
}
