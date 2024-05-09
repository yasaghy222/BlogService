using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlogService.Enums;

namespace BlogService.Entities
{
    public class Position : BaseEntity
    {
        public required string Key { get; set; }

        public Guid ForeignId { get; set; }

        public PositionType Type { get; set; } = PositionType.Banner;

        public Banner? Banner { get; set; }
        public Slide? Slide { get; set; }
        public Slider? Slider { get; set; }
        public Blog? Blog { get; set; }
    }
}