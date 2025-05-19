using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.src.Helpers.RequestHelpers
{
    /// <summary>
    /// Represents a paginated list of items.
    /// This class inherits from List<T> and includes metadata about the pagination.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class PagedList<T> : List<T>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PagedList{T}"/> class.
        /// This constructor is used to create a paginated list with the specified items, total count, page number, and page size.
        /// </summary>
        /// <param name="items">The items to include in the paginated list.</param>
        /// <param name="count">The total number of items in the source collection.</param>
        /// <param name="pageNumber">The current page number.</param>
        /// <param name="pageSize">The number of items per page.</param>
        public PagedList(List<T> items, int count, int pageNumber, int pageSize)
        {
            Metadata = new PaginationMetaData
            {
                TotalCount = count,
                PageSize = pageSize,
                CurrentPage = pageNumber,
                TotalPages = (int)Math.Ceiling(count / (double)pageSize)
            };

            AddRange(items);
        }

        public PaginationMetaData Metadata { get; set; }

        /// <summary>
        /// Creates a paginated list asynchronously from the specified source.
        /// This method retrieves the total count of items and the items for the specified page.
        /// </summary>
        /// <param name="source">The source queryable collection to paginate.</param>
        /// <param name="pageNumber">The current page number.</param>
        /// <param name="pageSize">The number of items per page.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the paginated list.</returns>
        public static async Task<PagedList<T>> CreateAsync(IQueryable<T> source, int pageNumber, int pageSize)
        {
            var count = await source.CountAsync();
            var items = await source.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();

            return new PagedList<T>(items, count, pageNumber, pageSize);
        }
    }
}