using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerce.src.Dto.User
{
    public class UserStatusDto
    {
        [StringLength(255, ErrorMessage = "La razón no puede tener más de 255 caracteres.")]
        public string? Reason { get; set; }

        [Required(ErrorMessage = "El estado es obligatorio.")]
        public bool Status { get; set; }
    }

}