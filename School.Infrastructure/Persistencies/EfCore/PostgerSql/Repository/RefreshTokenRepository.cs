using Microsoft.EntityFrameworkCore;
using School.Application.Repository;
using School.Domain.Entitys;

namespace School.Infrastructure.Persistencies.EfCore.PostgerSql.Repository
{
    public sealed class RefreshTokenRepository(SchoolDbContext context) : IRefreshTokenRepository
    {
        public Task<RefreshToken?> GetByHashAsync(string tokenHash, CancellationToken cancellationToken = default) =>
            context.RefreshTokens.FirstOrDefaultAsync(x => x.TokenHash == tokenHash, cancellationToken);

        public async Task AddAsync(RefreshToken token, CancellationToken cancellationToken = default) =>
            await context.RefreshTokens.AddAsync(token, cancellationToken);

        public async Task RevokeAllForUserAsync(ushort userId, CancellationToken cancellationToken = default)
        {
            var tokens = await context.RefreshTokens
                .Where(x => x.UserId == userId && x.RevokedAt == null)
                .ToListAsync(cancellationToken);
            foreach (var token in tokens)
                token.Revoke();
        }

        public async Task SaveChangesAsync(CancellationToken cancellationToken = default) =>
            await context.SaveChangesAsync(cancellationToken);
    }
}
