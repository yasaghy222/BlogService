using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlogService.Entities;
using BlogService.Enums;

namespace BlogService.Models
{
    public class SliderInfo
    {
        public Guid Id { get; set; }
        public string? Key { get; set; }
        public string? Description { get; set; }

        public ICollection<SlideInfo>? Slides { get; set; }
    }
}