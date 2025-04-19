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
    public class ProductRepository(StoreContext storeContext, IMapper mapper) : IProductRepository
    {
        private readonly StoreContext _storeContext = storeContext;
        private readonly IMapper _mapper = mapper;
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