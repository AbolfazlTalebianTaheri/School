using School.Application.Repository;
using School.Domain.Entitys;
using System;
using System.Collections.Generic;
using System.Text;

namespace School.Infrastructure.Persistencies.EfCore.PostgerSql.Repository
{
    public class LessonRepository(SchoolDbContext dbContext) : RepositoryBase<Lesson, ushort>(dbContext),ILessonRepository
    {
    }
}
