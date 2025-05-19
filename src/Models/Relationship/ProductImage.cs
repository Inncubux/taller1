using System.ComponentModel.DataAnnotations;

using ECommerce.src.Models;

namespace taller1.src.Models.Relationship
{
    /// <summary>
    /// Entity representing the relationship between a product and its images.
    /// This class is used to store the images associated with a product.
    /// </summary>
    public class ProductImage
    {
        // Primary key
        public int Id { get; set; }

        // Product ID (FK)
        public int ProductId { get; set; }

        // Property for navigation to the product
        public Product Product { get; set; } = null!;

        // Image URL of the product
        public required string ImageUrl { get; set; }

        public string PublicId { get; set; } = string.Empty;
    }
}