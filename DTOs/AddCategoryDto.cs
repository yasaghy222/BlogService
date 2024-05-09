using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlogService.Enums;

namespace BlogService.DTOs
{
    public class AddCategoryDto
    {
        public Guid? ParentId { get; set; }

        public required string EnTitle { get; set; }
        public required string Title { get; set; }

        public string? Description { get; set; }
    }
}