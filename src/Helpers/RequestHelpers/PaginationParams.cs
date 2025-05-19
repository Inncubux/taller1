using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerce.src.Helpers.RequestHelpers
{
    /// <summary>
    /// Represents pagination parameters for paginated API requests.
    /// Allows specifying the page number and page size, with a maximum page size limit.
    /// </summary>
    public class PaginationParams
    {
        /// <summary>
        /// The maximum allowed page size.
        /// </summary>
        private const int maxPageSize = 50;

        /// <summary>
        /// The current page number (default is 1).
        /// </summary>
        public int PageNumber { get; set; } = 1;

        private int _pageSize = 8;

        /// <summary>
        /// The number of items per page (default is 8, maximum is 50).
        /// </summary>
        public int PageSize
        {
            get => _pageSize;
            set => _pageSize = (value > maxPageSize) ? maxPageSize : value;
        }
    }
}