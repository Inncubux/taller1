using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using taller1.src.Models.Relationship;

namespace ECommerce.src.Dto
{
    public class GetProductDto
    {
        public required string Title { get; set; }
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public int Stock { get; set; }
        public string? Category { get; set; }
        public string? Brand { get; set; }
        public required string Condition { get; set; }

    }
}