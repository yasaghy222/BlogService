using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlogService.Entities;
using BlogService.Enums;

namespace BlogService.Models
{
    public class SlideInfo
    {
        public required string ImagePath { get; set; }
        public Guid Id { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public string? Link { get; set; }
        public int? Order { get; set; }
    }
}