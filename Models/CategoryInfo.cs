using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlogService.Entities;
using BlogService.Enums;

namespace BlogService.Models
{
    public class CategoryInfo
    {
        public Guid Id { get; set; }
        public Guid? ParentId { get; set; }

        public required string EnTitle { get; set; }
        public required string Title { get; set; }

        public string? Description { get; set; }

        public ICollection<CategoryInfo>? Chields { get; set; }
    }
}