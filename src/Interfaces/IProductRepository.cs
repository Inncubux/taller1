using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using ECommerce.src.Controllers;
using ECommerce.src.Dto;
using ECommerce.src.Models;

namespace ECommerce.src.Interfaces
{
    public interface IProductRepository
    {
        Task<IEnumerable<GetProductDto>> GetProductsAsync(string? sort);
    }
}