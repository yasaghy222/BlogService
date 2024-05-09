using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlogService.Entities
{
    public class SliderSlide : BaseEntity
    {
        public Guid SlideId { get; set; }
        public required Slide Slide { get; set; }

        public Guid SliderId { get; set; }
        public required Slider Slider { get; set; }
    }
}