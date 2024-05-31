using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlogService.Entities;
using BlogService.Enums;

namespace BlogService.Models
{
    public class AuthorDetail : BaseStatus<AuthorStatus>
    {
        public required string ImagePath { get; set; }
        public required string Title { get; set; }
        public required string Description { get; set; }

        public ICollection<BlogInfo>? Blogs { get; set; }
    }
}