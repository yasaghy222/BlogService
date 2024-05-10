using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlogService.DTOs
{
    public class EditSliderDto
    {
        public Guid Id { get; set; }
        public string? Key { get; set; }
        public string? Description { get; set; }
    }
}