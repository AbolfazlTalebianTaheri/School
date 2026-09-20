using School.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace School.Domain.Entitys
{
    public class AttendanceDelay : BaseClass<int>
    {
        public int AttendanceLogId { get; private set; }
        public AttendanceLog AttendanceLog { get; private set; } = null!;
        public Class_Bell Class_Bell { get; private set; }
        public ushort LessonId { get; private set; }
        public Lesson Lesson { get; private set; } = null!;
        public int DelayMinutes { get; private set; }
        private AttendanceDelay() { }
        public AttendanceDelay(Class_Bell class_Bell, ushort lessonId, int delayMinutes)
        {
            Class_Bell = class_Bell;
            LessonId = lessonId;
            DelayMinutes = delayMinutes;
        }
    }
}
