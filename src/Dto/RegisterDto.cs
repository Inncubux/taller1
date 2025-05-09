using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;


namespace ECommerce.src.Dto
{
    public class RegisterDto
    {
        [Required(ErrorMessage = "El nombre es obligatorio")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "El nombre debe tener al menos 3 carácteres.")]
        public required string FirstName { get; set; }
        [Required(ErrorMessage = "El apellido es obligatorio")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "El apellido debe tener al menos 3 carácteres.")]
        public required string LastName { get; set; }
        [Required(ErrorMessage = "El correo electrónico es obligatorio")]
        [EmailAddress(ErrorMessage = "El formato del correo electrónico no es válido")]
        public required string Email { get; set; }

        public required string Phone { get; set; }

        [Required(ErrorMessage = "La fecha de nacimiento es obligatoria")]
        public required DateTime BirthDate { get; set; }


        [Required(ErrorMessage = "La contraseña es obligatoria.")]
        [StringLength(100, MinimumLength = 8, ErrorMessage = "La contraseña debe tener al menos 8 caracteres.")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[!@#$%^&*()_+={}\[\]|\\:;\""<>,.?/~`]).+$",
        ErrorMessage = "La contraseña debe tener al menos una letra mayúscula, una letra minúscula, un número y un carácter especial.")]
        public required string Password { get; set; }
        [Required(ErrorMessage = "La contraseña es obligatoria.")]
        [StringLength(100, MinimumLength = 8, ErrorMessage = "La contraseña debe tener al menos 8 caracteres.")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[!@#$%^&*()_+={}\[\]|\\:;\""<>,.?/~`]).+$",
        ErrorMessage = "La contraseña debe tener al menos una letra mayúscula, una letra minúscula, un número y un carácter especial.")]
        public required string ConfirmPassword { get; set; }

        public required string? Street { get; set; }
        public required string? Number { get; set; }
        public required string? Commune { get; set; }
        public required string? Region { get; set; }
        public required string? PostalCode { get; set; }
    }
}