using System.ComponentModel.DataAnnotations;

using ECommerce.src.Models;

namespace taller1.src.Models.Relationship
{
    // 4. Imagen_Producto
    // Almacena las URLs de las imágenes asociadas con un producto.
    public class ProductImage
    {
        // ID de la imagen (PK)
        public int Id { get; set; }

        // ID del producto (FK)
        public int ProductId { get; set; }

        // Propiedad de navegación hacia el producto
        public Product Product { get; set; } = null!;

        // URL de la imagen (requerida)
        [Required]
        public required string ImageUrl { get; set; }
    }
}