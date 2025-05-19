using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using ECommerce.src.Helpers;
using ECommerce.src.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

using taller1.src.Dto.Product;
using taller1.src.Helpers;
using taller1.src.Interfaces;

namespace taller1.src.Services
{
    public class CloudinaryService : ICloudinaryService
    {
        private readonly Cloudinary _cloudinary;

        public CloudinaryService(IOptions<CloudinarySettings> config)
        {
            var acc = new Account(
                config.Value.CloudName,
                config.Value.ApiKey,
                config.Value.ApiSecret
            );

            _cloudinary = new Cloudinary(acc);
        }

        public async Task<ImageUploadResultDto> UploadImageAsync(IFormFile file, string folder)
        {
            if (file == null || file.Length == 0)
                throw new ArgumentException("El archivo no puede estar vacío.");

            var permittedExtensions = new[] { ".jpg", ".jpeg", ".png", ".webp" };
            var extension = Path.GetExtension(file.FileName).ToLowerInvariant();

            if (!permittedExtensions.Contains(extension))
                throw new ArgumentException("Formato de imagen no válido. Solo se permiten .jpg, .jpeg, .png y .webp.");

            var permittedMimeTypes = new[] { "image/jpeg", "image/png", "image/webp" };
            if (!permittedMimeTypes.Contains(file.ContentType.ToLowerInvariant()))
                throw new ArgumentException("Tipo MIME no válido. Solo se permiten imágenes .jpg, .jpeg, .png y .webp.");

                    const long maxFileSize = 100 * 1024 * 1024; // 100 MB
            if (file.Length > maxFileSize)
            {
                throw new ArgumentException("La imagen excede el tamaño máximo permitido de 100 MB.");
            }

            var uploadParams = new ImageUploadParams
            {
                File = new FileDescription(file.FileName, file.OpenReadStream()),
                Folder = folder,
                Transformation = new Transformation().Quality("auto").FetchFormat("auto")
            };

            var uploadResult = await _cloudinary.UploadAsync(uploadParams);

            if (uploadResult.Error != null)
                throw new ApplicationException(uploadResult.Error.Message);

            return new ImageUploadResultDto
            {
                Url = uploadResult.SecureUrl.AbsoluteUri,
                PublicId = uploadResult.PublicId
            };
        }


        public async Task DeleteImageAsync(string publicId)
        {
            if (string.IsNullOrEmpty(publicId))
                return; // No hay imagen que eliminar, salir sin error.

            var deleteParams = new DeletionParams(publicId);
            var result = await _cloudinary.DestroyAsync(deleteParams);

            // Manejar casos cuando la imagen no existe o ya fue eliminada
            if (result.Result == "not found")
            {
            
                return;
            }
            else if (result.Result != "ok")
            {
                // Si es otro error distinto, podrías lanzar excepción o loggear según tu política
                throw new ApplicationException($"Error al eliminar imagen en Cloudinary: {result.Result}");
            }
        }
        

    }
}