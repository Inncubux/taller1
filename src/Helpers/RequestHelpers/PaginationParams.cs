using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerce.src.Helpers.RequestHelpers
{
    public class PaginationParams
    {
        private const int maxPageSize = 50;

        public int PageNumber { get; set; } = 1;
        private int _pageSize = 8;

        public int PageSize
        {
            get => _pageSize;
            set => _pageSize = (value > maxPageSize) ? maxPageSize : value;
        }
    }
}