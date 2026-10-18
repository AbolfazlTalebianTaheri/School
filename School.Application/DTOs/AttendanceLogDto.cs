using School.Domain.Entitys;
using School.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace School.Application.DTOs
{
    public class AttendanceLogDto
    {
        public int Id { get; set; }
        public ushort StudentId { get; set; }
        public Student Student { get; set; } 
        public int AttendanceDayId { get; set; }
        public AttendanceDay AttendanceDay { get; set; }
        public bool IsAbsent { get; set; }
    }
}
