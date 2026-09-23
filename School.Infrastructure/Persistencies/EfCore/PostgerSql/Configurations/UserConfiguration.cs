using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.Extensions.Configuration;
using School.Domain.Constants;
using School.Domain.Entitys;
using System;
using System.Collections.Generic;
using System.Text;

namespace School.Infrastructure.Persistencies.EfCore.PostgerSql.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.UserName)
                .HasMaxLength(UserConstant.MaxLenghUserName);
            builder.Property(x => x.PassWordHash)
                .HasMaxLength(UserConstant.MaxLenghPasswordHash);
            builder.Property(x => x.UserRole)
                .IsRequired(true);
        }
    }
}
