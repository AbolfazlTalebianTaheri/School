using School.Domain.InterFaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace School.Domain.Entitys
{
    public class BaseClass<T> : ISoftDeletable
    {
        public T Id { get; set; }
        public DateTimeOffset CreationTime => DateTimeOffset.Now;
        public bool IsDeleted { get; set; }
    }
}
