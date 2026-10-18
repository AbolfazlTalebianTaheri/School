using System;
using System.Collections.Generic;
using System.Text;

namespace School.Domain.Exceptions
{
    public class DuplicateLogForStudentInDateException() : Exception("لاگ برای این دانش آموز قبلا در این تاریخ ثبت شده است!!")
    {

    }
}
