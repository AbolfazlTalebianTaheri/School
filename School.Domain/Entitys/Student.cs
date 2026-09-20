using School.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace School.Domain.Entitys
{
    public class Student : BaseClass<ushort>
    {
        public string FirstName { get; private set; } = null!;
        public string LastName { get; private set; } = null!;
        public int StudentCode { get; private set; }
        public List<User> Users { get; private set; } = new List<User>();
        public Grade_Level Grade_Level { get; private set; }
        public Field_Of_Study Field_Of_Study { get; private set; }
        private Student() { }
        public Student(string firstName, string lastName, int studentCode, Grade_Level grade_Level, Field_Of_Study field_Of_Study)
        {
            FirstName = firstName;
            LastName = lastName;
            StudentCode = studentCode;
            Grade_Level = grade_Level;
            Field_Of_Study = field_Of_Study;
        }
        public void AddPatern(User patern) => Users.Add(patern);
        public void DeletePatern(User patern) => Users.Remove(patern);
    }
}