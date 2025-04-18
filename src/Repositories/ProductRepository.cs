using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using ECommerce.src.Data;
using ECommerce.src.Interfaces;

namespace ECommerce.src.Repositories
{
    public class ProductRepository(StoreContext storeContext) :IProductRepository
    {

    }
}