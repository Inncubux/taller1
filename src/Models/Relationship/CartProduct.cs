using ECommerce.src.Models;

namespace taller1.src.Models.Relationship
{
    public class CartProduct
    {
        // Clave primaria surrogate
        public int Id { get; set; }

        // ID del carrito (FK)
        public int CartId { get; set; }

        // Propiedad de navegación hacia el carrito
        public Cart Cart { get; set; } = null!;

        // ID del producto (FK)
        public int ProductId { get; set; }

        // Propiedad de navegación hacia el producto
        public Product Product { get; set; } = null!;

        // Cantidad del producto en el carrito
        public int Quantity { get; set; }
    }
}