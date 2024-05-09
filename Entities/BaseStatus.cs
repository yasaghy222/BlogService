using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlogService.Entities
{
    public class BaseStatus<T> : BaseEntity
    {
        public virtual required T Status { get; set; }
        public virtual string? StatusDescription { get; set; }
    }
}