using System.ComponentModel.DataAnnotations;

using taller1.src.Models.Relationship;

namespace ECommerce.src.Models
{
    // 3. Producto
    // Representa los productos disponibles en la plataforma.
    public class Product
    {
        // ID del producto (PK)
        public int Id { get; set; }

        // Título del producto
        [Required]
        public required string Title { get; set; }

        // Descripción del producto
        public string? Description { get; set; }

        // Precio del producto
        public decimal Price { get; set; }

        // Stock disponible
        public int Stock { get; set; }

        // Categoría del producto
        public string? Category { get; set; }

        // Marca del producto
        public string? Brand { get; set; }

        // Condición del producto ("nuevo" o "usado")
        [Required]
        public required string Condition { get; set; }

        // Fecha de creación del producto
        public DateTime CreationDate { get; set; } = DateTime.Now;

        // Fecha de modificación del producto
        public DateTime? ModificationDate { get; set; }

        // Lista de imágenes asociadas al producto.
        public List<ProductImage> Images { get; set; } = new();

        // (Opcional) Lista de relaciones con carritos.
        // Si decides que un producto "sepa" dónde aparece en los carritos, puedes usar esta lista.
        public List<CartProduct> CartProducts { get; set; } = new();

        // (Opcional) Lista de relaciones con pedidos.
        // Esto puede ser útil para análisis o auditorías.
        public List<OrderProduct> OrderProducts { get; set; } = new();
    }
}