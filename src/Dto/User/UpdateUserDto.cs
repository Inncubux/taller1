using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerce.src.Dto.User
{
    /// <summary>
    /// Data Transfer Object for updating user profile information.
    /// Contains only the fields that can be updated by the user.
    /// Fields left as null will not be updated.
    /// </summary>
    public class UpdateUserDto
    {
        /// <summary>
        /// User's first name (optional).
        /// </summary>
        [StringLength(100, MinimumLength = 3, ErrorMessage = "El nombre debe tener al menos 3 carácteres.")]
        public string? FirstName { get; set; }

        /// <summary>
        /// User's last name (optional).
        /// </summary>
        [StringLength(100, MinimumLength = 3, ErrorMessage = "El apellido debe tener al menos 3 carácteres.")]
        public string? LastName { get; set; }

        /// <summary>
        /// User's email address (optional).
        /// </summary>
        [EmailAddress(ErrorMessage = "El formato del correo electrónico no es válido")]
        public string? Email { get; set; }

        /// <summary>
        /// User's phone number (optional).
        /// </summary>
        public string? Phone { get; set; }

        /// <summary>
        /// User's date of birth (optional).
        /// </summary>
        public DateTime? BirthDate { get; set; }

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