using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlogService.Models
{
    public class AuthorInfo
    {
        public required string ImagePath { get; set; }
        public Guid Id { get; set; }
        public required string Title { get; set; }
        public required string Description { get; set; }

        public ICollection<BlogInfo>? Blogs { get; set; }
    }
}