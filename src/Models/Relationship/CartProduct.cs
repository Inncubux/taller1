using ECommerce.src.Models;

namespace taller1.src.Models.Relationship
{
    /// <summary>
    /// Entity representing the relationship between a cart and a product.
    /// </summary>
    public class CartProduct
    {
        // primary key
        public int Id { get; set; }

        // Cart ID (FK)
        public int CartId { get; set; }

        // Property for navigation to the cart
        public Cart Cart { get; set; } = null!;

        // Product ID (FK)
        public int ProductId { get; set; }

        // Property for navigation to the product
        public Product Product { get; set; } = null!;

        // Quantity of the product in the cart
        public int Quantity { get; set; }
    }
}