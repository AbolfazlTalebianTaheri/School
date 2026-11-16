using School.Domain.Entitys;

namespace School.Application.Repository
{
    public interface IRefreshTokenRepository
    {
        Task<RefreshToken?> GetByHashAsync(string tokenHash, CancellationToken cancellationToken = default);
        Task AddAsync(RefreshToken token, CancellationToken cancellationToken = default);
        Task RevokeAllForUserAsync(ushort userId, CancellationToken cancellationToken = default);
        Task SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
