using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlogService.Entities;
using BlogService.Enums;

namespace BlogService.Models
{
    public class BannerDetail : BaseStatus<BannerStatus>
    {
        public string? Title { get; set; }
        public string? Description { get; set; }
        public string? Link { get; set; }

        public ICollection<BlogInfo>? Blogs { get; set; }
    }
}