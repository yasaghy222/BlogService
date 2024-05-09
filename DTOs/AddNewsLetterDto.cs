using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlogService.DTOs
{
    public class AddNewsLetterDto
    {
        public required string Phone { get; set; }
        public required string Email { get; set; }
    }
}