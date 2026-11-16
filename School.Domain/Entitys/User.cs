using School.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace School.Domain.Entitys
{
    public class User : BaseClass<ushort>
    {
        public string? UserName { get; private set; }
        public string? PassWordHash { get; private set; }
        public ushort? StudentId { get; private set; }
        public List<Student> Students { get; private set; } = new();
        public List<Lesson> Lessons { get; private set; } = new();
        public UserRole UserRole { get; private set; }
        private User() { }
        public User(string? userName, UserRole userRole, string? passWord, ushort? studentId = null)
        {
            UserName = userName;
            UserRole = userRole;
            PassWordHash = passWord;
            StudentId = studentId;
        }
        public void AddLesson(Lesson lesson) =>
            Lessons.Add(lesson);
        public void AddStudent(Student student) =>
            Students.Add(student);
    }
}
