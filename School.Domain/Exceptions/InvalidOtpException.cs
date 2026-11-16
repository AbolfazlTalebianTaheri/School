using System;
using System.Collections.Generic;
using System.Text;

namespace School.Domain.Exceptions
{
    public class InvalidOtpException(string message)
    : Exception(message);
}
