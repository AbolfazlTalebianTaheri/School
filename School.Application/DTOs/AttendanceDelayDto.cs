using School.Domain.Entitys;
using School.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace School.Application.DTOs
{
    public class AttendanceDelayDto
    {
        public int AttendanceLogId { get; set; }
        public Class_Bell Class_Bell { get; set; }
        public int LessonId { get; set; }
        public int DelayMinutes { get; set; }
    }
}
