using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerce.src.Helpers.RequestHelpers
{
    /// <summary>
    /// Represents filtering and pagination parameters for querying users.
    /// Inherits pagination properties and adds filters for status, registration date range, and search term.
    /// </summary>
    public class UserParams : PaginationParams
    {
        /// <summary>
        /// Filter by user status (true for active, false for inactive, null for all).
        /// </summary>
        public bool? Status { get; set; }

        /// <summary>
        /// Filter users registered from this date (inclusive).
        /// </summary>
        public DateTime? RegistrationDateFrom { get; set; }

        /// <summary>
        /// Filter users registered up to this date (inclusive).
        /// </summary>
        public DateTime? RegistrationDateTo { get; set; }

        /// <summary>
        /// Search term to filter users by name, email, or other fields.
        /// </summary>
        public string? SearchTerm { get; set; }
    }
}