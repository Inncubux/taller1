using ECommerce.src.Models;

using taller1.src.Models.Relationship;

namespace taller1.src.Models
{
    /// <summary>
    /// Entity representing a shopping cart.
    /// This class is used to store the products that a user intends to purchase.
    /// </summary>
    public class Cart
    {
        // Primary key
        public int Id { get; set; }

        // User ID (FK)
        public string UserId { get; set; } = null!;

        // Property for navigation to the user
        public User User { get; set; } = null!;

        // List of products in the cart
        public List<CartProduct> Products { get; set; } = new();
    }
}