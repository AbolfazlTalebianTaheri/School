using School.Domain.Entitys;
using School.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace School.Application.DTOs
{
    public class StudentDto
    {
        public string FirstName { get;  set; } = null!;
        public string LastName { get;  set; } = null!;
        public int StudentCode { get;  set; }
        public List<User> Users{ get;  set; } = new List<User>();
        public Grade_Level Grade_Level { get;  set; }
        public Field_Of_Study Field_Of_Study { get;  set; }
    }
}
