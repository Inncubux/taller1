using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using ECommerce.src.Dto.Product;
using ECommerce.src.Models;

namespace taller1.src.Mappers
{
    public class ProductMapper
    {
        
        public static GetProductDto ProductToDto(Product product) =>
            new GetProductDto
            {
                Id = product.Id,
                Title = product.Title,
                Description = product.Description,
                Price = product.Price,
                Stock = product.Stock,
                Category = product.Category,
                Brand = product.Brand,
                Condition = product.Condition,
                ImageUrls = product.Images?.Select(pi => pi.ImageUrl).ToList() ?? new List<string>()
            };

    }
}