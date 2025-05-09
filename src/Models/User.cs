using System.ComponentModel.DataAnnotations;

using Microsoft.AspNetCore.Identity;

using taller1.src.Models;

namespace ECommerce.src.Models
{
    /// <summary>
    /// Entity representing a user in the system.
    /// </summary>
    public class User : IdentityUser
    {
        // First name (required)
        public required string FirstName { get; set; }
        // Last name (required)

        public required string LastName { get; set; }
        //Phone number (required)
        public required string Phone { get; set; }

        // Birth date (required)
        public required DateTime BirthDate { get; set; }

        // Password (required)
        public required string Password { get; set; }


        // Status: 0: inactive, 1: active (required)
        public bool Status { get; set; } = true;

        // Creation date of the user (default is the current date) required
        public required DateTime RegistrationDate { get; set; } = DateTime.Now;

        // Last login date (optional)
        public DateTime? LastLogin { get; set; }

        // Relationship :
        // Address: 1 user can have many addresses.
        public Address? Address { get; set; }

        public string? DeactivationReason { get; set; }

        // Cart: 1 user can have 1 cart.
        public Cart? Cart { get; set; }

        // Order: 1 user can have many orders.
        public List<Order>? Orders { get; set; }
    }
}