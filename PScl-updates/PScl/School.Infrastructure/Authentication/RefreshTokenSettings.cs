namespace School.Infrastructure.Authentication
{
    public class RefreshTokenSettings
    {
        public int SessionLifetimeHours { get; set; } = 12;
        public int PersistentLifetimeDays { get; set; } = 30;
        public int ReuseLeewaySeconds { get; set; } = 10;
        public int RetentionDays { get; set; } = 7;
        public int CleanupIntervalHours { get; set; } = 6;
    }
}
