using School.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace School.Application.DTOs
{
    public class LessonDto
    {
        public string LessonName { get;  set; } = null!;
        public Grade_Level Grade_Level { get;  set; }
        public Field_Of_Study Field_Of_Study { get;  set; }
    }
}
