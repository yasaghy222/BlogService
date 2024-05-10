using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using BlogService.Enums;

namespace BlogService.Entities
{
    public class Author : BaseEntity
    {
        public required string Title { get; set; }
        public required string Description { get; set; }
        public AuthorStatus Status { get; set; } = AuthorStatus.UnConfirmed;
        public string? StatusDescription { get; set; }

        [JsonIgnore]
        public ICollection<Blog>? Blogs { get; set; }
    }
}