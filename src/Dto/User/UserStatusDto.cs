using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerce.src.Dto.User
{
    /// <summary>
    /// Data Transfer Object for updating a user's status (active/inactive).
    /// Contains the new status and an optional reason for deactivation.
    /// </summary>
    public class UserStatusDto
    {
        /// <summary>
        /// Optional reason for deactivation (up to 255 characters).
        /// </summary>
        [StringLength(255)]
        public string? Reason { get; set; }

        /// <summary>
        /// Indicates the new status of the user (true for active, false for inactive).
        /// </summary>
        public bool Status { get; set; }
    }
}