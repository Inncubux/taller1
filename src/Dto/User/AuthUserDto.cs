using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerce.src.Dto.User
{
    /// <summary>
    /// Data Transfer Object representing an authenticated user.
    /// Contains user profile information and the JWT token for authentication.
    /// </summary>
    public class AuthUserDto
    {
        /// <summary>
        /// User's first name.
        /// </summary>
        public string FirtsName { get; set; } = null!;

        /// <summary>
        /// User's last name.
        /// </summary>
        public string LastName { get; set; } = null!;

        /// <summary>
        /// User's email address.
        /// </summary>
        public string Email { get; set; } = null!;

        /// <summary>
        /// User's telephone number.
        /// </summary>
        public string Thelephone { get; set; } = null!;

        /// <summary>
        /// JWT authentication token.
        /// </summary>
        public string Token { get; set; } = null!;

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

        /// <summary>
        /// Date when the user registered.
        /// </summary>
        public DateTime RegistrationDate { get; set; }

        /// <summary>
        /// Date and time of the user's last login (optional).
        /// </summary>
        public DateTime? LastLogin { get; set; }

        /// <summary>
        /// Indicates whether the user account is active.
        /// </summary>
        public bool Status { get; set; }
    }
}