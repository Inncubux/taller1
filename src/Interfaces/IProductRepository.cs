using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using ECommerce.src.Controllers;
using ECommerce.src.Dto.Product;
using ECommerce.src.Models;

using taller1.src.Dto.Product;

namespace ECommerce.src.Interfaces
{
    /// <summary>
    /// Interface for the Product repository.
    /// </summary>
    public interface IProductRepository
    {

        Task<bool> DeleteProductAsync(int id);

        Task<GetProductDto> AddProductAsync(CreateProductDto newProductDto, string folder);
        Task<GetProductDto> GetProductByIdAsync(int id);


        /// <summary>
        /// Asynchronously retrieves a list of products from the database.
        /// </summary>
        /// <param name="sort">Sort the product</param>
        /// <returns>All products that match the sort option</returns>
        Task<IEnumerable<GetProductDto>> GetProductsAsync(string? sort);
        Task<bool> UpdateProductImageAsync(int productId, int imageId, IFormFile newImage, string folder);
    }
}