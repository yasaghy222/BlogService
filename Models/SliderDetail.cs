using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlogService.Entities;
using BlogService.Enums;

namespace BlogService.Models
{
    public class SliderDetail : BaseEntity
    {
        public string? Key { get; set; }
        public string? Description { get; set; }

        public ICollection<SlideDetail>? Slides { get; set; }
    }
}