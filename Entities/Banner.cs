using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlogService.Enums;

namespace BlogService.Entities
{
    public class Banner : BaseEntity
    {
        public required string ImagePath { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }

        public string? Link { get; set; }

        public BannerStatus Status { get; set; } = BannerStatus.Hide;

        public ICollection<Position>? Positions { get; set; }
    }
}