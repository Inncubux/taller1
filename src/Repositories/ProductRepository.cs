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

using taller1.src.Dto.Product;
using taller1.src.Mappers;
using taller1.src.Models.Relationship;

namespace ECommerce.src.Repositories
{
    /// <summary>
    /// Repository class for managing products in the database.
    /// This class implements the IProductRepository interface and provides methods for retrieving products.
    /// </summary>
    public class ProductRepository(StoreContext storeContext, IMapper mapper, PhotoService photoService) : IProductRepository
    {
        // StoreContext is a database context that provides access to the database.
        private readonly StoreContext _storeContext = storeContext;
        // IMapper is an interface for mapping between domain models and DTOs.
        private readonly IMapper _mapper = mapper;

        private readonly PhotoService _photoService = photoService;


        /// <summary>
        /// Method to retrieve a list of products from the database.
        /// This method supports sorting the products by price in ascending or descending order.
        /// </summary>
        /// <param name="sort">Optional sort option</param>
        /// <returns>Products that match the sort option</returns>
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
            return products.Select(ProductMapper.ProductToDto);

        }

        /// <summary>
        /// Deletes a product from the database by its ID.
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns> <summary>
        /// 
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        public async Task<bool> DeleteProductAsync(int productId)
        {
            var product = await _storeContext.Products.Include(p => p.Images)
                .FirstOrDefaultAsync(p => p.Id == productId);

            if (product == null) return false;

            foreach (var image in product.Images)
            {
                await _photoService.DeletePhotoAsync(image.PublicId);
            }

            _storeContext.Products.Remove(product);

            return await _storeContext.SaveChangesAsync() > 0;
        }

        public async Task<bool> UpdateProductImageAsync(int productId, int imageId, IFormFile newImageFile, string folder)
        {
            // Buscar producto con sus imágenes
            var product = await _storeContext.Products
                .Include(p => p.Images)
                .FirstOrDefaultAsync(p => p.Id == productId);

            if (product == null) return false;

            // Buscar la imagen a actualizar
            var imageToUpdate = product.Images.FirstOrDefault(i => i.Id == imageId);
            if (imageToUpdate == null) return false;

            // Eliminar imagen previa en Cloudinary
            if (!string.IsNullOrEmpty(imageToUpdate.PublicId))
            {
                await _photoService.DeletePhotoAsync(imageToUpdate.PublicId);
            }

            // Subir nueva imagen
            var uploadResult = await _photoService.AddPhotoAsync(newImageFile, folder);

            // Actualizar datos
            imageToUpdate.ImageUrl = uploadResult.SecureUrl.ToString();
            imageToUpdate.PublicId = uploadResult.PublicId;

            // Guardar cambios en base de datos
            var result = await _storeContext.SaveChangesAsync();

            return result > 0;
        }

        public async Task<GetProductDto> AddProductAsync(CreateProductDto newProductDto, string folder)
        {
            // Subir imagen a Cloudinary usando PhotoService
            var uploadResult = await _photoService.AddPhotoAsync(newProductDto.ImageFile, folder);

            // Crear producto con todos los campos y la imagen subida
            var product = new Product
            {
                Title = newProductDto.Title,
                Description = newProductDto.Description,
                Price = newProductDto.Price,
                Stock = newProductDto.Stock,
                Category = newProductDto.Category,
                Brand = newProductDto.Brand,
                Condition = newProductDto.Condition,
                CreationDate = DateTime.Now,
                Images = new List<ProductImage>
                {
                    new ProductImage
                    {
                        ImageUrl = uploadResult.SecureUrl.ToString(),
                        PublicId = uploadResult.PublicId
                    }
                }
            };

            // Agregar y guardar en la base de datos
            await _storeContext.Products.AddAsync(product);
            await _storeContext.SaveChangesAsync();

            // Mapear a DTO para retornar
            return ProductMapper.ProductToDto(product);
        }

        public async Task<GetProductDto> GetProductByIdAsync(int id)
        {
            var product = await _storeContext.Products
                .Include(p => p.Images)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (product == null) return null;

            return ProductMapper.ProductToDto(product);
        }

    }
}