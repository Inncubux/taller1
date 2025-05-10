using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerce.src.Dto.User
{
    public class UpdateUserDto
    {
        [Required(ErrorMessage = "El nombre es obligatorio")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "El nombre debe tener al menos 3 carácteres.")]
        public required string FirstName { get; set; }

        [Required(ErrorMessage = "El apellido es obligatorio")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "El apellido debe tener al menos 3 carácteres.")]
        public required string LastName { get; set; }

        [EmailAddress(ErrorMessage = "El formato del correo electrónico no es válido")]
        public required string Email { get; set; }

        public required string Phone { get; set; }

        public required DateTime BirthDate { get; set; }

        public required string? Street { get; set; }
        public required string? Number { get; set; }
        public required string? Commune { get; set; }
        public required string? Region { get; set; }
        public required string? PostalCode { get; set; }
    }
}