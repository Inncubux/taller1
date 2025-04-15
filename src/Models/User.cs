using System.ComponentModel.DataAnnotations;

using taller1.src.Models;

namespace ECommerce.src.Models
{
    // 1. Usuario
    // Representa a los usuarios registrados (Cliente o Administrador).
    public class User
    {
        // ID del usuario (PK)
        public int Id { get; set; }

        // Nombre completo (requerido)
        [Required]
        public required string FullName { get; set; }

        // Correo electrónico (único y requerido)
        [Required, EmailAddress]
        public required string Email { get; set; }

        // Teléfono
        public string? Phone { get; set; }

        // Fecha de nacimiento (requerida)
        [Required]
        public DateTime BirthDate { get; set; }

        // Contraseña segura (requerida)
        [Required]
        public required string Password { get; set; }

        // Rol: "cliente" o "administrador" (requerido)
        [Required]
        public required int Role { get; set; }

        // Estado de la cuenta: "activo" o "inactivo"
        public int Status { get; set; } = 0;

        // Fecha de registro (por defecto la fecha actual)
        public DateTime RegistrationDate { get; set; } = DateTime.Now;

        // Último acceso (nullable si nunca ha iniciado sesión)
        public DateTime? LastLogin { get; set; }

        // Relaciones:
        // Lista de direcciones asociadas al usuario.
        public List<Address> Addresses { get; set; } = new();

        // Carrito de compras del usuario.
        public Cart Cart { get; set; } = new();

        // Historial de pedidos del usuario.
        public List<Order> Orders { get; set; } = new();
    }
}