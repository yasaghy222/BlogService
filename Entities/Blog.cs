using BlogService.Enums;

namespace BlogService.Entities
{
    public class Blog : BaseEntity
    {
        public required string ImagePath { get; set; }
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
        public required Author Author { get; set; }

        public Guid CategoryId { get; set; }
        public required Category Category { get; set; }

        public BlogStatus Status { get; set; } = BlogStatus.InProses;
        public DateTime? PublishedDate { get; set; }
        public string? StatusDescription { get; set; }

        public ICollection<Position>? Positions { get; set; }
    }
}