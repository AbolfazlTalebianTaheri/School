using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using School.Domain.Entitys;

namespace School.Infrastructure.Persistencies.EfCore.PostgerSql.Configurations
{
    public class RefreshTokenConfiguration : IEntityTypeConfiguration<RefreshToken>
    {
        public void Configure(EntityTypeBuilder<RefreshToken> builder)
        {
            builder.ToTable("RefreshTokens");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.TokenHash)
                .HasMaxLength(64)
                .IsRequired();
            builder.HasIndex(x => x.TokenHash).IsUnique();
            builder.HasOne(x => x.User)
                .WithMany()
                .HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.Cascade);
            builder.HasIndex(x => x.TokenHash)
                .IsUnique();
        }
    }
}
