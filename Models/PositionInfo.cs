using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlogService.Entities;
using BlogService.Enums;

namespace BlogService.Models
{
    public class PositionInfo : BaseEntity
    {
        public required string Key { get; set; }

        public Guid ForeignId { get; set; }

        public PositionType Type { get; set; }

        public BannerInfo? Banner { get; set; }
        public SliderInfo? Slider { get; set; }
        public BlogTitle? Blog { get; set; }
    }
}