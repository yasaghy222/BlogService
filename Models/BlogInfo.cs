using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlogService.Models
{
    public class BlogInfo
    {
        public required string ImagePath { get; set; }
        public Guid Id { get; set; }
        public required string EnTitle { get; set; }
        public required string Title { get; set; }
        public required string Description { get; set; }
        public required string Content { get; set; }

        public required string Tags { get; set; }
        public required string ResourceLinks { get; set; }
        public int? SuggestedReadingTime { get; set; }
        public int? CommentsCount { get; set; } = 0;
        public int? VisitCount { get; set; } = 0;

        public Guid AuthorId { get; set; }
        public required string AuthorTitle { get; set; }

        public Guid CategoryId { get; set; }
        public required string CategoryTitle { get; set; }
        public required string CategoryEnTitle { get; set; }

        public DateTime? PublishedDate { get; set; }
    }
}