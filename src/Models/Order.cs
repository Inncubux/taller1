using ECommerce.src.Models;

using taller1.src.Models.Relationship;

namespace taller1.src.Models
{
    /// <summary>
    /// Entity representing an order.
    /// This class is used to store the details of a user's order.
    /// </summary>
    public class Order
    {
        // Primary key
        public int Id { get; set; }

        // User ID (FK) who made the order
        public string UserId { get; set; } = null!;
        public User User { get; set; } = null!;

        // Address ID (FK) for delivery
        public int AddressId { get; set; }
        public Address Address { get; set; } = null!;

        // Date and time of the order
        public DateTime PurchaseDate { get; set; } = DateTime.Now;

        // Total amount of the order
        public decimal Total { get; set; }

        // Status of the order (e.g., pending, shipped, delivered)
        public string OrderStatus { get; set; } = "pending";

        // List of products in the order
        public List<OrderProduct> OrderProducts { get; set; } = new();
    }
}