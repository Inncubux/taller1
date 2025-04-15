using System.ComponentModel.DataAnnotations;

using ECommerce.src.Models;

namespace taller1.src.Models
{
    // 2. Dirección
    // Almacena la información de las direcciones de envío de los usuarios.
    public class Address
    {
        // ID de la dirección (PK)
        public int Id { get; set; }

        // ID del usuario (FK)
        public int UserId { get; set; }

        // Propiedad de navegación hacia el usuario
        public User User { get; set; } = null!;

        // Calle
        [Required]
        public required string Street { get; set; }

        // Número
        [Required]
        public required string Number { get; set; }

        // Comuna
        [Required]
        public required string Commune { get; set; }

        // Región
        [Required]
        public required string Region { get; set; }

        // Código postal
        [Required]
        public required string PostalCode { get; set; }

        // ¿Es la dirección predeterminada? (bool)
        public bool IsDefault { get; set; } = false;
    }
}