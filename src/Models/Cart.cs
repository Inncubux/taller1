using ECommerce.src.Models;
using taller1.src.Models.Relationship;

namespace taller1.src.Models
{
    // 5. Carrito
    // Representa el contenedor de productos que un usuario desea comprar.
    // Aquí se almacenan los productos y la cantidad deseada.
    public class Cart
    {
        // ID del carrito (PK)
        public int Id { get; set; }

        // ID del usuario (FK)
        public int UserId { get; set; }

        // Propiedad de navegación hacia el usuario dueño del carrito
        public User User { get; set; } = null!;

        // Lista de productos en el carrito con su cantidad.
        public List<CartProduct> Products { get; set; } = new();
    }
}