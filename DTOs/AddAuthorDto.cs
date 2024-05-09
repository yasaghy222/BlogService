using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlogService.Enums;

namespace BlogService.DTOs
{
    public class AddAuthorDto
    {
        public required string Title { get; set; }
        public required string Description { get; set; }
    }
}