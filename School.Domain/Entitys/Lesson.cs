using School.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace School.Domain.Entitys
{
    public class Lesson : BaseClass<ushort>
    {
        public string LessonName { get; private set; } = null!;
        public Grade_Level Grade_Level { get; private set; }
        public Field_Of_Study Field_Of_Study { get; private set; }
        private Lesson() { }
        public Lesson(string lessonName, Grade_Level grade_Level, Field_Of_Study field_Of_Study)
        {
            LessonName = lessonName;
            Grade_Level = grade_Level;
            Field_Of_Study = field_Of_Study;
        }
    }
}
