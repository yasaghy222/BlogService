using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlogService.Entities;
using BlogService.Enums;

namespace BlogService.Models
{
    public class CategoryDetail : BaseEntity
    {
        public Guid? ParentId { get; set; }
        public CategoryParent? Parent { get; set; }

        public required string EnTitle { get; set; }
        public required string Title { get; set; }

        public string? Description { get; set; }

        public ICollection<BlogInfo>? Blogs { get; set; }
    }
}