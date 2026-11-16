using System;
using System.Collections.Generic;
using System.Text;

namespace School.Domain.Exceptions
{
    public sealed class OtpRateLimitException(string message) : Exception(message);
}
