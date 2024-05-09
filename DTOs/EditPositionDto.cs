using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlogService.Enums;

namespace BlogService.DTOs
{
    public class EditPositionDto
    {
        public Guid Id { get; set; }
        public required string Key { get; set; }

        public Guid ForeignId { get; set; }

        public PositionType Type { get; set; } = PositionType.Banner;
    }
}