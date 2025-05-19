using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using AutoMapper;

using ECommerce.src.Data;
using ECommerce.src.Dto.Product;
using ECommerce.src.Interfaces;
using ECommerce.src.Models;

using Microsoft.EntityFrameworkCore;

using taller1.src.Interfaces;
using taller1.src.Models.Relationship;

namespace ECommerce.src.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly StoreContext _storeContext;
        private readonly IMapper _mapper;
        private readonly ICloudinaryService _cloudinaryService;

        public ProductRepository(StoreContext storeContext, IMapper mapper, ICloudinaryService cloudinaryService)
        {
            _storeContext = storeContext;
            _mapper = mapper;
            _cloudinaryService = cloudinaryService;
        }

        public async Task<IEnumerable<GetProductDto>> GetProductsAsync(string? sort)
        {
            IQueryable<Product> productQuery = _storeContext.Products.Include(p => p.Images);

            if (sort is not null)
                productQuery = sort switch
                {
                    "asc" => productQuery.OrderBy(p => p.Price),
                    "desc" => productQuery.OrderByDescending(p => p.Price),
                    _ => productQuery
                };

            var products = await productQuery.ToListAsync();
            return _mapper.Map<IEnumerable<GetProductDto>>(products);
        }

        public async Task<bool> DeleteProductAsync(int productId)
        {
            var product = await _storeContext.Products
                .Include(p => p.Images)
                .FirstOrDefaultAsync(p => p.Id == productId);

            if (product == null) return false;

            // Eliminar imágenes en Cloudinary
            foreach (var image in product.Images)
            {
                if (!string.IsNullOrEmpty(image.ImageUrl))
                {
                    // Asumiendo que PublicId está almacenado en otra propiedad, si no, extrae PublicId desde la URL.
                    var publicId = ExtractPublicIdFromUrl(image.ImageUrl);
                    if (!string.IsNullOrEmpty(publicId))
                    {
                        await _cloudinaryService.DeleteImageAsync(publicId);
                    }
                }
            }

            // Eliminar imágenes de la BD
            _storeContext.RemoveRange(product.Images);
            // Eliminar producto
            _storeContext.Products.Remove(product);

            await _storeContext.SaveChangesAsync();

            return true;
        }

        // Método auxiliar para extraer PublicId desde la URL
        private string ExtractPublicIdFromUrl(string url)
        {

            try
            {
                var uri = new Uri(url);
                var segments = uri.Segments;
                // Busca la parte después de "/upload/"
                var uploadIndex = Array.IndexOf(segments, "upload/");

                if (uploadIndex == -1)
                {
                    // Alternativa: buscar segmento que contenga "upload"
                    uploadIndex = segments.ToList().FindIndex(s => s.Contains("upload"));
                    if (uploadIndex == -1) return string.Empty;
                }

                // Los segmentos después de upload/ son el publicId con posible extensión
                var publicIdSegments = segments.Skip(uploadIndex + 1).ToArray();
                var publicIdWithExt = string.Join("", publicIdSegments);

                // Eliminar la extensión del archivo
                var publicId = publicIdWithExt.Substring(0, publicIdWithExt.LastIndexOf('.'));
                return publicId.Trim('/');
            }
            catch
            {
                return string.Empty;
            }
        }
        
        public async Task<bool> UpdateProductImageAsync(int productId, IFormFile newImageFile, string folder)
        {
            var product = await _storeContext.Products
                .Include(p => p.Images)
                .FirstOrDefaultAsync(p => p.Id == productId);

            if (product == null)
                return false;

            var existingImage = product.Images.FirstOrDefault();

            if (existingImage != null)
            {
                var oldPublicId = ExtractPublicIdFromUrl(existingImage.ImageUrl);
                if (!string.IsNullOrEmpty(oldPublicId))
                    await _cloudinaryService.DeleteImageAsync(oldPublicId);
            }

            var newImageUrl = await _cloudinaryService.UploadImageAsync(newImageFile, folder);

            if (existingImage != null)
            {
                existingImage.ImageUrl = newImageUrl;
                _storeContext.Images.Update(existingImage);
            }
            else
            {
                var newProductImage = new ProductImage
                {
                    ProductId = productId,
                    ImageUrl = newImageUrl
                };
                await _storeContext.Images.AddAsync(newProductImage);
            }

            await _storeContext.SaveChangesAsync();
            return true;
        }

    }
}
