using Microsoft.EntityFrameworkCore;
using School.Domain.Entitys;
using System;
using System.Collections.Generic;
using System.Text;

namespace School.Infrastructure.Persistencies.EfCore.PostgerSql
{
    public class SchoolDbContext(DbContextOptions<SchoolDbContext> options) : DbContext(options)
    {
        public DbSet<Lesson> Lessons { get; set; }
        public DbSet<Student> Students { get; set; }
        public DbSet<User> Users{ get; set; }
        public DbSet<AttendanceDelay> AttendanceDelays { get; set; }
        public DbSet<AttendanceLog> AttendanceLogs { get; set; }
        public DbSet<AttendanceDay> AttendanceDays { get; set; }

    }
}
