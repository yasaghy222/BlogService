using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlogService.Enums;

namespace BlogService.Entities
{
    public class Slide : BaseEntity
    {
        public string? Title { get; set; }
        public string? Description { get; set; }
        public string? Link { get; set; }
        public int? Order { get; set; } = 0;

        public SlideStatus Status { get; set; } = SlideStatus.Hide;

        public Guid SliderId { get; set; }
        public required Slider Slider { get; set; }
    }
}