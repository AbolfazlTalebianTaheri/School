using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using School.Domain.Constants;
using School.Domain.Entitys;
using System;
using System.Collections.Generic;
using System.Text;

namespace School.Infrastructure.Persistencies.EfCore.PostgerSql.Configurations
{
    public class StudentConfiguration : IEntityTypeConfiguration<Student>
    {
        public void Configure(EntityTypeBuilder<Student> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.FirstName)
                .HasMaxLength(StudentConstant.MaxLenghFirstName)
                .IsUnicode(true)
                .IsRequired();
            builder.Property(x => x.LastName)
                .HasMaxLength(StudentConstant.MaxLenghLastName)
                .IsUnicode(true)
                .IsRequired();
            builder.Property(x => x.StudentCode)
                .IsRequired(true);
            builder.Property(x => x.Users)
                .IsRequired(true);
            builder.Property(x => x.Grade_Level)
                .IsRequired(true);
            builder.Property(x => x.Field_Of_Study)
                .IsRequired(true);
            builder.HasMany(x => x.Users)
                .WithOne(x => x.Student)
                .HasForeignKey(x => x.StudentId);
        }
    }
}
