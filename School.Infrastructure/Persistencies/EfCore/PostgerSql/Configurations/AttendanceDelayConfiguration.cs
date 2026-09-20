using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using School.Domain.Entitys;
using System;
using System.Collections.Generic;
using System.Text;

namespace School.Infrastructure.Persistencies.EfCore.PostgerSql.Configurations
{
    public class AttendanceDelayConfiguration : IEntityTypeConfiguration<AttendanceDelay>
    {
        public void Configure(EntityTypeBuilder<AttendanceDelay> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.DelayMinutes)
                .IsRequired();
            builder.Property(x => x.Class_Bell)
                .IsRequired()
                .HasConversion<byte>();
            builder.HasOne(x => x.AttendanceLog)
                .WithMany(x => x.Delays)
                .HasForeignKey(x => x.AttendanceLogId)
                .OnDelete(DeleteBehavior.Cascade);
            builder.HasOne(x => x.Lesson)
                .WithMany()
                .HasForeignKey(x => x.LessonId)
                .OnDelete(DeleteBehavior.Restrict);
            builder.HasIndex(x => new
            {
                x.AttendanceLogId,
                x.Class_Bell
            })
            .IsUnique();
        }
    }
}
