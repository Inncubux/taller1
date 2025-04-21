using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using AutoMapper;

using ECommerce.src.Data;
using ECommerce.src.Dto;
using ECommerce.src.Interfaces;
using ECommerce.src.Models;

using Microsoft.EntityFrameworkCore;

namespace ECommerce.src.Repositories
{
    /// <summary>
    /// Repository class for managing products in the database.
    /// This class implements the IProductRepository interface and provides methods for retrieving products.
    /// </summary>
    public class ProductRepository(StoreContext storeContext, IMapper mapper) : IProductRepository
    {
        // StoreContext is a database context that provides access to the database.
        private readonly StoreContext _storeContext = storeContext;
        // IMapper is an interface for mapping between domain models and DTOs.
        private readonly IMapper _mapper = mapper;


        /// <summary>
        /// Method to retrieve a list of products from the database.
        /// This method supports sorting the products by price in ascending or descending order.
        /// </summary>
        /// <param name="sort">Optional sort option</param>
        /// <returns>Products that match the sort option</returns>
        public async Task<IEnumerable<GetProductDto>> GetProductsAsync(string? sort)
        {
            IQueryable<Product> productQuery = _storeContext.Products;

            if (sort is not null)
                productQuery = sort switch
                {
                    "asc" => productQuery.OrderBy(p => p.Price),
                    "desc" => productQuery.OrderByDescending(p => p.Price),
                    _ => productQuery
                };

            var products = await productQuery.ToListAsync();
            return _mapper.Map<IEnumerable<GetProductDto>>(products);
        }
    }
}