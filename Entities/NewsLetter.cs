using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlogService.Entities
{
    public class NewsLetter : BaseEntity
    {
        public required string Email { get; set; }
        public required string Phone { get; set; }
    }
}