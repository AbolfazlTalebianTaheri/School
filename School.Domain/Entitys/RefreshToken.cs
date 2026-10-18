using System;
using System.Collections.Generic;
using System.Text;

namespace School.Domain.Entitys
{
    public class RefreshToken
    {
        public int Id { get; private set; }
        public string TokenHash { get; private set; } = null!;
        public string UserId { get; private set; } = null!;
        public DateTime ExpiresAtUtc { get; private set; }
        public DateTime CreatedAtUtc { get; private set; }
        public DateTime? RevokedAtUtc { get; private set; }
        public int? ReplacedByTokenId { get; private set; }
        private RefreshToken() { }
        public RefreshToken(string tokenHash, string userId, DateTime expiresAtUtc)
        {
            TokenHash = tokenHash;
            UserId = userId;
            ExpiresAtUtc = expiresAtUtc;
            CreatedAtUtc = DateTime.UtcNow;
        }
        public bool IsActive => RevokedAtUtc is null && DateTime.UtcNow < ExpiresAtUtc;
        public void Revoke()
        {
            if (RevokedAtUtc is null)
                RevokedAtUtc = DateTime.UtcNow;
        }
    }
}
