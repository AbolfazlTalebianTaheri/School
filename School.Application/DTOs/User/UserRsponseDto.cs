using School.Domain.Entitys;
using School.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace School.Application.DTOs.User
{
    public class UserRsponseDto
    {
        public UserRole UserRole { get; set; }
        public string? UserName { get; set; }
        public string? Password{ get; set; }
        public string? NationalCode { get; set; }
        public ushort? StudentId { get; set; }
        public List<Lesson> Lessons { get; set; }
    }
}
