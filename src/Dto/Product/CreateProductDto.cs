using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace taller1.src.Dto.Product
{
    public class CreateProductDto
    {
        [Required]
        public string Title { get; set; }

        public string? Description { get; set; }

        public decimal Price { get; set; }

        public int Stock { get; set; }

        public string? Category { get; set; }

        public string? Brand { get; set; }

        [Required]
        public string Condition { get; set; }

        // Imagen que subirás a Cloudinary
        public IFormFile ImageFile { get; set; }
    }

}