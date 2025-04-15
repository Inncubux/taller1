using ECommerce.src.Models;

using taller1.src.Models.Relationship;

namespace taller1.src.Models
{
    // 7. Pedido
    // Registra los pedidos realizados por los clientes, incluyendo información de pago y envío.
    public class Order
    {
        // ID del pedido (PK)
        public int Id { get; set; }

        // ID del usuario (FK)
        public int UserId { get; set; }
        public User User { get; set; } = null!;

        // ID de la dirección (FK) usada para el envío
        public int AddressId { get; set; }
        public Address Address { get; set; } = null!;

        // Fecha de compra
        public DateTime PurchaseDate { get; set; } = DateTime.Now;

        // Precio total del pedido
        public decimal Total { get; set; }

        // Estado del pedido (pendiente, enviado, entregado, cancelado)
        public string OrderStatus { get; set; } = "pending";

        // Lista de productos incluidos en el pedido con su cantidad y precio.
        public List<OrderProduct> OrderProducts { get; set; } = new();
    }
}