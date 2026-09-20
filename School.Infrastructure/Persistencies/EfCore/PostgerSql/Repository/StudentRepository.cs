using School.Application.Repository;
using School.Domain.Entitys;
using System;
using System.Collections.Generic;
using System.Text;

namespace School.Infrastructure.Persistencies.EfCore.PostgerSql.Repository
{
    public class StudentRepository(SchoolDbContext dbContext) : RepositoryBase<Student,ushort>(dbContext),IStudentRepository
    {
    }
}
