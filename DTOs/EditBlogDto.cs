using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlogService.Enums;

namespace BlogService.DTOs
{
    public class EditBlogDto
    {
        public IFormFile? Image { get; set; }
        public Guid Id { get; set; }
        public required string EnTitle { get; set; }
        public required string Title { get; set; }
        public required string Description { get; set; }
        public required string Content { get; set; }

        public required string Tags { get; set; }
        public required string ResourceLinks { get; set; }
        public int? SuggestedReadingTime { get; set; }

        public Guid CategoryId { get; set; }
    }
}