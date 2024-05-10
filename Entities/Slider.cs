using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlogService.Enums;

namespace BlogService.Entities
{
    public class Slider : BaseEntity
    {
        public string? Key { get; set; }
        public string? Description { get; set; }
        public SliderStatus Status { get; set; }

        public ICollection<Slide>? Slides { get; set; }
        public ICollection<Position>? Positions { get; set; }
    }
}