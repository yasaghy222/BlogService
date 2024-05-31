using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlogService.DTOs
{
    public class EditAuthorDto
    {
        public IFormFile? Image { get; set; }
        public Guid Id { get; set; }
        public required string Title { get; set; }
        public required string Description { get; set; }
    }
}