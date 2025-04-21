using ECommerce.src.Models;

namespace taller1.src.Models.Relationship
{
    /// <summary>
    /// Entity representing the relationship between a cart and a product.
    /// </summary>
    public class OrderProduct
    {

        // Primary key
        public int Id { get; set; }

        // Order ID (FK)
        public int OrderId { get; set; }
        public Order Order { get; set; } = null!;

        // Product ID (FK)
        public int ProductId { get; set; }
        public Product Product { get; set; } = null!;

        // Quantity of the product in the order
        public int Quantity { get; set; }

        // Unit price of the product at the time of order
        public decimal UnitPrice { get; set; }
    }
}