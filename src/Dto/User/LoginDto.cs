using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerce.src.Dto.User
{
    /// <summary>
    /// Data Transfer Object for user login.
    /// Contains the email and password required for authentication.
    /// </summary>
    public class LoginDto
    {
        /// <summary>
        /// The user's email address.
        /// </summary>
        public required string Email { get; set; }

        /// <summary>
        /// The user's password.
        /// </summary>
        public required string Password { get; set; }
    }
}