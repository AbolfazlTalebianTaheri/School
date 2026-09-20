using School.Domain.Entitys;
using School.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace School.Application.DTOs
{
    public class UserDto
    {
        public string? UserName { get; set; }
        public string? PhoneNumber { get; set; }
        public ushort? StudentId { get; set; }
        public Student? Student { get; set; }
        public UserRole UserRole { get; set; }
        public string? PassWordHash { get; set; }
    }
}
