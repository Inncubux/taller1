using ECommerce.src.Models;

namespace taller1.src.Models.Relationship
{
    // 8. Pedido_Producto
    // Relaciona cada pedido con sus productos, registrando la cantidad y precio unitario.
    public class OrderProduct
    {
        
        // Clave primaria surrogate
        public int Id { get; set; }

        // ID del pedido (FK)
        public int OrderId { get; set; }
        public Order Order { get; set; } = null!;

        // ID del producto (FK)
        public int ProductId { get; set; }
        public Product Product { get; set; } = null!;

        // Cantidad del producto en el pedido
        public int Quantity { get; set; }

        // Precio unitario del producto en el momento de la compra
        public decimal UnitPrice { get; set; }
    }
}