using School.Domain.Entitys;
using School.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace School.Application.DTOs
{
    public class AttendanceLogDto
    {
        public ushort StudentId { get; private set; }
        public int AttendanceDayId { get; private set; }
        public bool IsAbsent { get; private set; } //  If absence has done or no
    }
}
