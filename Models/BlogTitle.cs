using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlogService.Models
{
    public class BlogTitle
    {
        public required string ImagePath { get; set; }
        public Guid Id { get; set; }
        public required string Title { get; set; }
        public required string EnTitle { get; set; }
    }
}