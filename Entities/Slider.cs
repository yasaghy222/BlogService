using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlogService.Entities
{
    public class Slider
    {
        public string? Title { get; set; }
        public string? Description { get; set; }

        public ICollection<SliderSlide>? SliderSlides { get; set; }
        public ICollection<Position>? Positions { get; set; }
    }
}