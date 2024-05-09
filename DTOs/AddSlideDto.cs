using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlogService.DTOs
{
    public class AddSlideDto
    {
        public string? Title { get; set; }
        public string? Description { get; set; }
        public string? Link { get; set; }
        public int? Order { get; set; } = 0;
    }
}