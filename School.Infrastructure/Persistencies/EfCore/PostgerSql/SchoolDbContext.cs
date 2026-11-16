using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using School.Domain.InterFaces;
using School.Domain.Entitys;
using School.Infrastructure.Persistencies.EfCore.PostgerSql.Configurations;
using System;
using System.Collections.Generic;
using System.Text;

namespace School.Infrastructure.Persistencies.EfCore.PostgerSql
{
    public class SchoolDbContext(DbContextOptions<SchoolDbContext> options) : DbContext(options)
    {
        public DbSet<Lesson> Lessons { get; set; }
        public DbSet<Student> Students { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<AttendanceDelay> AttendanceDelays { get; set; }
        public DbSet<AttendanceLog> AttendanceLogs { get; set; }
        public DbSet<AttendanceDay> AttendanceDays { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfiguration(new RefreshTokenConfiguration());
            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                if (typeof(ISoftDeletable)
                    .IsAssignableFrom(entityType.ClrType))
                {
                    var parameter = Expression.Parameter(
                        entityType.ClrType, "e");

                    var property = Expression.Property(
                        parameter, nameof(ISoftDeletable.IsDeleted));

                    var condition = Expression.Equal(
                        property, Expression.Constant(false));

                    var filter = Expression.Lambda(
                        condition, parameter);

                    modelBuilder.Entity(entityType.ClrType)
                        .HasQueryFilter(filter);

                }
            }
        }
    }
}
