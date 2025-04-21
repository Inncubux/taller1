using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using ECommerce.src.Controllers;
using ECommerce.src.Dto;
using ECommerce.src.Models;

namespace ECommerce.src.Interfaces
{
    /// <summary>
    /// Interface for the Product repository.
    /// </summary>
    public interface IProductRepository
    {
        /// <summary>
        /// Asynchronously retrieves a list of products from the database.
        /// </summary>
        /// <param name="sort">Sort the product</param>
        /// <returns>All products that match the sort option</returns>
        Task<IEnumerable<GetProductDto>> GetProductsAsync(string? sort);
    }
}