using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerce.src.Dto.User
{
    /// <summary>
    /// Data Transfer Object for changing the user's password.
    /// Contains the current password, the new password, and the confirmation of the new password.
    /// </summary>
    public class ChangePasswordDto
    {
        /// <summary>
        /// The current password of the user.
        /// </summary>
        public required string Password { get; set; }

        [Required(ErrorMessage = "La contraseña es obligatoria.")]
        [StringLength(100, MinimumLength = 8, ErrorMessage = "La contraseña debe tener al menos 8 caracteres.")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[!@#$%^&*()_+={}\[\]|\\:;\""<>,.?/~`]).+$",
        ErrorMessage = "La contraseña debe tener al menos una letra mayúscula, una letra minúscula, un número y un carácter especial.")]
        public required string NewPassword { get; set; }

        [Required(ErrorMessage = "La contraseña es obligatoria.")]
        [StringLength(100, MinimumLength = 8, ErrorMessage = "La contraseña debe tener al menos 8 caracteres.")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[!@#$%^&*()_+={}\[\]|\\:;\""<>,.?/~`]).+$",
ErrorMessage = "La contraseña debe tener al menos una letra mayúscula, una letra minúscula, un número y un carácter especial.")]
        public required string ConfirmPassword { get; set; }

    }
}