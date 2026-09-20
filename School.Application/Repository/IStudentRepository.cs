using School.Domain.Entitys;
using System;
using System.Collections.Generic;
using System.Text;

namespace School.Application.Repository
{
    public interface IStudentRepository : IRepository<Student, ushort>
    {

    }
}
