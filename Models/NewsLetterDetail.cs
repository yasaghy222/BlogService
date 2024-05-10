using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlogService.Entities;

namespace BlogService.Models
{
    public class NewsLetterDetail : BaseEntity
    {
        public required string Phone { get; set; }
        public required string Email { get; set; }
    }
}