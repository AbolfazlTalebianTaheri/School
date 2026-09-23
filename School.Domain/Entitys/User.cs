using School.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace School.Domain.Entitys
{
    public class User : BaseClass<ushort>
    {
        public string? UserName { get; private set; }
        public string? PhoneNumber { get; private set; }
        public ushort? StudentId { get; private set; }
        public Student? Student { get; private set; }
        public UserRole UserRole { get; private set; }
        public string? PassWordHash { get; private set; }
        private User() { }
        public User(string? phoneNumber, string? userName, UserRole userRole, string? passWordHash,ushort studentId)
        {
            UserName = userName;
            PhoneNumber = phoneNumber;
            UserRole = userRole;
            PasswordHash = passWordHash;
            StudentId = studentId; 
        }
    }
}
