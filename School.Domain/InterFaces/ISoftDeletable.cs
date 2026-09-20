using System;
using System.Collections.Generic;
using System.Text;

namespace School.Domain.InterFaces
{
    public interface ISoftDeletable
    {
        public bool IsDeleted { get; set; }
    }
}
