using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using School.Domain.Entitys;
using System;
using System.Collections.Generic;
using System.Reflection.Emit;
using System.Text;

namespace School.Infrastructure.Persistencies.EfCore.PostgerSql.Configurations
{
    public class AttendanceLogConfiguration : IEntityTypeConfiguration<AttendanceLog>
    {
        public void Configure(EntityTypeBuilder<AttendanceLog> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.StudentId)
                .IsRequired();
            builder.Property(x => x.AttendanceDayId)
                .IsRequired();
            builder.Property(x => x.IsAbsent)
                .IsRequired()
                .HasDefaultValue(false);
            builder.HasOne(x => x.Student)
                .WithMany()
                .HasForeignKey(x => x.StudentId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(x => x.AttendanceDay)
                .WithMany(x => x.Logs)
                .HasForeignKey(x => x.AttendanceDayId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);
            builder.HasMany(x => x.Delays)
                .WithOne(x => x.AttendanceLog)
                .HasForeignKey(x => x.AttendanceLogId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);
            builder.Navigation(x => x.Delays)
                .UsePropertyAccessMode(
                    PropertyAccessMode.Field);
            builder.HasIndex(x => new
            {
                x.AttendanceDayId,
                x.StudentId
            })
            .IsUnique();
            builder.HasIndex(x => new
            {
                x.AttendanceDayId,
                x.IsAbsent
            });
        }
    }
}

