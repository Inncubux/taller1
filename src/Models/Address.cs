using System.ComponentModel.DataAnnotations;

using ECommerce.src.Models;

namespace taller1.src.Models
{
    /// <summary>
    /// Entity representing a user's address.
    /// </summary>
    public class Address
    {
        // Primary key
        public int Id { get; set; }

        // User ID (FK)
        public int UserId { get; set; }

        // Property for navigation to the user
        public User User { get; set; } = null!;

        // Street
        [Required]
        public required string Street { get; set; }

        // Number
        [Required]
        public required string Number { get; set; }

        // City
        [Required]
        public required string Commune { get; set; }

        // Region
        [Required]
        public required string Region { get; set; }

        // Postal code
        [Required]
        public required string PostalCode { get; set; }

        // Indicates if this address is the default address for the user
        public bool IsDefault { get; set; } = false;
    }
}