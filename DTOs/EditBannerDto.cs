using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlogService.DTOs
{
    public class EditBannerDto
    {
        public IFormFile? Image { get; set; }
        public Guid Id { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }

        public string? Link { get; set; }
    }
}