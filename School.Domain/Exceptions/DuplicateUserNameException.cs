using System;
using System.Collections.Generic;
using System.Text;

namespace School.Domain.Exceptions
{
    public class DuplicateUserNameException() : Exception("A user with this username already exists")
    {
    }
}
