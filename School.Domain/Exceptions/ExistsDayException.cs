using System;
using System.Collections.Generic;
using System.Text;

namespace School.Domain.Exceptions
{
    public class ExistsDayException(string message = "روز از قبل وجود دارد") : Exception(message)
    {
    }
}
