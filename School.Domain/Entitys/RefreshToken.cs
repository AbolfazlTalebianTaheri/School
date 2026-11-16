namespace School.Domain.Entitys
{
    public class RefreshToken
    {
        public long Id { get; private set; }
        public ushort UserId { get; private set; }
        public User User { get; private set; }
        public string TokenHash { get; private set; } = null!;
        public bool IsPersistent { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public DateTime ExpiresAt { get; private set; }
        public DateTime? RevokedAt { get; private set; }
        public string? ReplacedByTokenHash { get; private set; }

        private RefreshToken() { }

        public RefreshToken(
            string tokenHash,
            ushort userId,
            DateTime expiresAt,
            bool isPersistent = false)
        {
            TokenHash = tokenHash;
            UserId = userId;
            ExpiresAt = expiresAt;
            IsPersistent = isPersistent;
            CreatedAt = DateTime.UtcNow;
        }

        public bool IsExpired => DateTime.UtcNow >= ExpiresAt;
        public bool IsRevoked => RevokedAt.HasValue;
        public bool IsActive => !IsExpired && !IsRevoked;

        public void Revoke(string? replaceByTokenHash = null)
        {
            RevokedAt = DateTime.UtcNow;
            ReplacedByTokenHash = replaceByTokenHash;
        }
    }
}
