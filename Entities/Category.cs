using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlogService.Entities
{
    public class Category : BaseEntity
    {
        public Guid? ParentId { get; set; }
        public Category? Parent { get; set; }
        public ICollection<Category>? Chields { get; set; }

        public required string EnTitle { get; set; }
        public required string Title { get; set; }

        public string? Description { get; set; }

        public ICollection<Blog>? Blogs { get; set; }
    }
}