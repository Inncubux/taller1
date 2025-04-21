using System.ComponentModel.DataAnnotations;

using taller1.src.Models;

namespace ECommerce.src.Models
{
    /// <summary>
    /// Entity representing a user in the system.
    /// </summary>
    public class User
    {
        // Primary key
        public int Id { get; set; }

        // Full name of the user (required)
        [Required]
        public required string FullName { get; set; }

        // Email address (required and must be unique)
        [Required, EmailAddress]
        public required string Email { get; set; }

        // Phone number (optional)
        public string? Phone { get; set; }

        // Birth date (required)
        [Required]
        public DateTime BirthDate { get; set; }

        // Password (required)
        [Required]
        public required string Password { get; set; }

        // Rol: 1: admin, 2: user, 3: guest (required)
        [Required]
        public required int Role { get; set; }

        // Status: 0: inactive, 1: active (required)
        public int Status { get; set; } = 0;

        // Creation date of the user (default is the current date)
        public DateTime RegistrationDate { get; set; } = DateTime.Now;

        // Last login date (optional)
        public DateTime? LastLogin { get; set; }

        // Relationship :
        // Address: 1 user can have many addresses.
        public List<Address> Addresses { get; set; } = new();

        // Cart: 1 user can have 1 cart.
        public Cart Cart { get; set; } = new();

        // Order: 1 user can have many orders.
        public List<Order> Orders { get; set; } = new();
    }
}