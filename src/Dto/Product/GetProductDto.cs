using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using taller1.src.Models.Relationship;

namespace ECommerce.src.Dto.Product
{
    /// <summary>
    /// Model for retrieving product information.
    /// This class is used to transfer product data between the application and the client.
    /// </summary>
    public class GetProductDto
    {
        /// <summary>
        /// Name of the product.
        /// </summary>
        public required string Title { get; set; }
        /// <summary>
        /// Description of the product.
        /// /// This field is optional and can be null.
        /// </summary>
        public string? Description { get; set; }
        /// <summary>
        /// Price of the product.
        /// </summary>
        public decimal Price { get; set; }
        /// <summary>
        /// count of the product in stock.
        /// </summary>
        public int Stock { get; set; }
        /// <summary>
        /// Category of the product.
        /// Optional field that can be null.
        /// </summary>
        public string? Category { get; set; }
        /// <summary>
        /// Brand of the product.
        /// </summary>
        public string? Brand { get; set; }
        /// <summary>
        /// Condition of the product.
        /// </summary>
        public required string Condition { get; set; }

    }
}