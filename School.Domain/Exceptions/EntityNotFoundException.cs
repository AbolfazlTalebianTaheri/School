using System;
using System.Collections.Generic;
using System.Text;

namespace School.Domain.Exceptions
{
    public class EntityNotFoundException(string entityName,object id) : Exception($"{entityName} with id '{id}' was not found")
    {
    }
}
