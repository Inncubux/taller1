using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using ECommerce.src.Interfaces;

using Microsoft.AspNetCore.Mvc;

using taller1.src.Dto.Product;

namespace ECommerce.src.Controllers
{
    /// <summary>
    /// Provides endpoints for managing products.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
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
            var deleted = await _productRepository.DeleteProductAsync(id);

            if (!deleted) return NotFound();

            return NoContent(); // 204
        }

        [HttpPut("{productId}/images/{imageId}")]
        public async Task<IActionResult> UpdateProductImage(int productId, int imageId, IFormFile newImage)
        {
            if (newImage == null)
                return BadRequest("Debe proporcionar un archivo de imagen.");

            var folder = "product_images"; // o la carpeta que uses en Cloudinary

            var updated = await _productRepository.UpdateProductImageAsync(productId, imageId, newImage, folder);

            if (!updated)
                return NotFound("Producto o imagen no encontrado.");

            return NoContent(); // 204 sin contenido pero exitosa
        }

        [HttpPost]
        public async Task<IActionResult> AddProduct([FromForm] CreateProductDto newProductDto)
        {
            if (newProductDto == null)
                return BadRequest("Datos del producto no pueden ser nulos.");

            var folder = "products"; // carpeta para Cloudinary

            var createdProduct = await _productRepository.AddProductAsync(newProductDto, folder);

            if (createdProduct == null)
                return BadRequest("No se pudo crear el producto.");

            // Asumiendo que tienes un método GetProductById para devolver el producto creado:
            return CreatedAtAction(nameof(GetProductById), new { id = createdProduct.Id }, createdProduct);
        }


        [HttpGet("{id}")]
        public async Task<IActionResult> GetProductById(int id)
        {
            var product = await _productRepository.GetProductByIdAsync(id);

            if (product == null)
                return NotFound();

            return Ok(product);
        }



    }
}