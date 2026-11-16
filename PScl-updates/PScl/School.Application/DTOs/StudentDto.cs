using School.Application.DTOs.User;
using School.Domain.Entitys;
using School.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace School.Application.DTOs
{
    public class StudentDto
    {
        public ushort Id { get; set; }
        public string FirstName { get;  set; } = null!;
        public string LastName { get;  set; } = null!;
        public int StudentCode { get;  set; }
        public string NationalCode { get; set; } = string.Empty;
        public List<UserRsponseDto> Users{ get;  set; } = new List<UserRsponseDto>();
        public Grade_Level Grade_Level { get;  set; }
        public Field_Of_Study Field_Of_Study { get;  set; }
    }
}
