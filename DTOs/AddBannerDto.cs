using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlogService.DTOs
{
    public class AddBannerDto
    {
        public string? Title { get; set; }
        public string? Description { get; set; }

        public string? Link { get; set; }
    }
}