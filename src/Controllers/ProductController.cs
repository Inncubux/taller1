using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using ECommerce.src.Interfaces;

using Microsoft.AspNetCore.Mvc;

namespace ECommerce.src.Controllers
{
    /// <summary>
    /// Provides endpoints for managing products.
    /// </summary>
    public class ProductController(IProductRepository productRepository) : BaseApiController
    {
        /// <summary>
        /// Variable to store the product repository instance.
        /// </summary>
        private readonly IProductRepository _productRepository = productRepository;
        /// <summary>
        /// Variable to store the sort options.
        /// </summary>
        /// <value></value>
        private readonly string[] _sortOptions = { "asc", "desc" };


        /// <summary>
        /// Endpoint to get a list of products.
        /// </summary>
        /// <param name="sort">Opcional param to sort products</param>
        /// <returns>Return all products that match with the sort option</returns>
        [HttpGet]
        public async Task<IActionResult> GetProducts([FromQuery] string? sort)
        {
            if (sort is not null)
            {
                sort = sort.ToLower();

                if (!_sortOptions.Contains(sort))
                {
                    return BadRequest("El valor del parámetro 'sort' debe ser 'asc' o 'desc'");
                }
            }
            var products = await _productRepository.GetProductsAsync(sort);

            return Ok(products);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            try
            {
                await _productRepository.DeleteProductAsync(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

    }
}