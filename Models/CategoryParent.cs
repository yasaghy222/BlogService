using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlogService.Models
{
    public class CategoryParent
    {
        public Guid Id { get; set; }
        public Guid? ParentId { get; set; }
        public CategoryParent? Parent { get; set; }

        public required string Title { get; set; }
        public required string EnTitle { get; set; }
    }
}