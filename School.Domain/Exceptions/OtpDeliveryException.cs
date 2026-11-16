namespace School.Domain.Exceptions
{
    /// <summary>ارسال پیامک توسط سرویس‌دهنده ناموفق بود.</summary>
    public class OtpDeliveryException(string message) : Exception(message);
}
