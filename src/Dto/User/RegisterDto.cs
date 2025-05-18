using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerce.src.Dto.User
{
    /// <summary>
    /// Data Transfer Object for user registration.
    /// Contains all the necessary information to create a new user account.
    /// </summary>
    public class RegisterDto
    {
        /// <summary>
        /// User's first name.
        /// </summary>
        [Required(ErrorMessage = "El nombre es obligatorio")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "El nombre debe tener al menos 3 carácteres.")]
        public required string FirstName { get; set; }

        /// <summary>
        /// User's last name.
        /// </summary>
        [Required(ErrorMessage = "El apellido es obligatorio")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "El apellido debe tener al menos 3 carácteres.")]
        public required string LastName { get; set; }

        /// <summary>
        /// User's email address.
        /// </summary>
        [Required(ErrorMessage = "El correo electrónico es obligatorio")]
        [EmailAddress(ErrorMessage = "El formato del correo electrónico no es válido")]
        public required string Email { get; set; }

        /// <summary>
        /// User's phone number.
        /// </summary>
        public required string Phone { get; set; }

        /// <summary>
        /// User's date of birth.
        /// </summary>
        [Required(ErrorMessage = "La fecha de nacimiento es obligatoria")]
        public required DateTime BirthDate { get; set; }

        /// <summary>
        /// User's password.
        /// Must be at least 8 characters long and contain at least one uppercase letter, one lowercase letter, one number, and one special character.
        /// </summary>
        [Required(ErrorMessage = "La contraseña es obligatoria.")]
        [StringLength(100, MinimumLength = 8, ErrorMessage = "La contraseña debe tener al menos 8 caracteres.")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[!@#$%^&*()_+={}\[\]|\\:;\""<>,.?/~`]).+$",
        ErrorMessage = "La contraseña debe tener al menos una letra mayúscula, una letra minúscula, un número y un carácter especial.")]
        public required string Password { get; set; }

        /// <summary>
        /// Confirmation of the user's password.
        /// Must match the password and follow the same validation rules.
        /// </summary>
        [Required(ErrorMessage = "La contraseña es obligatoria.")]
        [StringLength(100, MinimumLength = 8, ErrorMessage = "La contraseña debe tener al menos 8 caracteres.")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[!@#$%^&*()_+={}\[\]|\\:;\""<>,.?/~`]).+$",
        ErrorMessage = "La contraseña debe tener al menos una letra mayúscula, una letra minúscula, un número y un carácter especial.")]
        public required string ConfirmPassword { get; set; }

        /// <summary>
        /// User's street address (optional).
        /// </summary>
        public string? Street { get; set; }

        /// <summary>
        /// User's address number (optional).
        /// </summary>
        public string? Number { get; set; }

        /// <summary>
        /// User's commune or district (optional).
        /// </summary>
        public string? Commune { get; set; }

        /// <summary>
        /// User's region (optional).
        /// </summary>
        public string? Region { get; set; }

        /// <summary>
        /// User's postal code (optional).
        /// </summary>
        public string? PostalCode { get; set; }
    }
}