using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerce.src.Helpers.RequestHelpers
{
    /// <summary>
    /// Represents pagination metadata for paginated API responses.
    /// Contains information about the total number of items, page size, current page, and total pages.
    /// </summary>
    public class PaginationMetaData
    {
        /// <summary>
        /// Total number of items available.
        /// </summary>
        public int TotalCount { get; set; }

        /// <summary>
        /// Number of items per page.
        /// </summary>
        public int PageSize { get; set; }

        /// <summary>
        /// The current page number.
        /// </summary>
        public int CurrentPage { get; set; }

        /// <summary>
        /// Total number of pages available.
        /// </summary>
        public int TotalPages { get; set; }
    }
}