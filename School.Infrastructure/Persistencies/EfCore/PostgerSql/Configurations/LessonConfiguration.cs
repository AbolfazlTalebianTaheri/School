using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using School.Domain.Constants;
using School.Domain.Entitys;
using System;
using System.Collections.Generic;
using System.Text;

namespace School.Infrastructure.Persistencies.EfCore.PostgerSql.Configurations
{
    public class LessonConfiguration : IEntityTypeConfiguration<Lesson>
    {
        public void Configure(EntityTypeBuilder<Lesson> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.LessonName)
                .HasMaxLength(LessonConstant.MaxLenghLessonName)
                .IsUnicode(true)
                .IsRequired(true);
            builder.Property(x => x.Grade_Level)
                .IsRequired(true);
            builder.Property(x => x.Field_Of_Study)
                .IsRequired(true);

        }
    }
}
