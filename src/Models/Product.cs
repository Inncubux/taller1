using System.ComponentModel.DataAnnotations;

using taller1.src.Models.Relationship;

namespace ECommerce.src.Models
{
    /// <summary>
    /// Entity representing a product in the e-commerce system.
    /// </summary>
    public class Product
    {
        // Primary key
        public int Id { get; set; }

        // Product name
        [Required]
        public required string Title { get; set; }

        // Product description
        public string? Description { get; set; }

        // Price of the product
        public decimal Price { get; set; }

        // Product stock
        public int Stock { get; set; }

        // Product SKU (Stock Keeping Unit)
        public string? Category { get; set; }

        // Product brand
        public string? Brand { get; set; }

        // Product condition (new, used, refurbished, etc.)
        [Required]
        public required string Condition { get; set; }

        // Creation date of the product
        public DateTime CreationDate { get; set; } = DateTime.Now;

        // Modification date of the product
        public DateTime? ModificationDate { get; set; }

        // (Optional) List of images associated with the product.
        public List<ProductImage> Images { get; set; } = new();

        // (Optional) List of reviews associated with the product.
        public List<CartProduct> CartProducts { get; set; } = new();

        // (Optional) List of orders associated with the product.
        public List<OrderProduct> OrderProducts { get; set; } = new();
    }
}