using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using ECommerce.src.Interfaces;

using Microsoft.AspNetCore.Mvc;

namespace ECommerce.src.Controllers
{
    [Route("Product")]
    public class ProductController(IProductRepository productRepository) : BaseApiController
    {
        private readonly IProductRepository _productRepository = productRepository;
        private readonly string[] _sortOptions = { "asc", "desc" };


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

    }
}