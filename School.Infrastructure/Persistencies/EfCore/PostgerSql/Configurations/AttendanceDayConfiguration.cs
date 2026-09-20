using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using School.Domain.Entitys;
using System;
using System.Collections.Generic;
using System.Reflection.Emit;
using System.Text;

namespace School.Infrastructure.Persistencies.EfCore.PostgerSql.Configurations
{
    public class AttendanceDayConfiguration : IEntityTypeConfiguration<AttendanceDay>
    {
        public void Configure(EntityTypeBuilder<AttendanceDay> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Date)
                .IsRequired();
            builder.HasIndex(x => x.Date)
                .IsUnique();
            builder.HasMany(x => x.Logs)
                .WithOne(x => x.AttendanceDay)
                .HasForeignKey(x => x.AttendanceDayId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);
            builder.Navigation(x => x.Logs)
                .UsePropertyAccessMode(PropertyAccessMode.Field);
        }
    }
}