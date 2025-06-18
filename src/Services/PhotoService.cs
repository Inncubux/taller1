using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.Extensions.Options;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using taller1.src.Helpers;

public class PhotoService
{
    private readonly Cloudinary _cloudinary;

    public PhotoService(IOptions<CloudinarySettings> config)
    {
        var acc = new Account(
            config.Value.CloudName,
            config.Value.ApiKey,
            config.Value.ApiSecret);

        _cloudinary = new Cloudinary(acc);
    }

    public async Task<ImageUploadResult> AddPhotoAsync(IFormFile file, string folder)
    {
        if (file == null || file.Length == 0)
        {
            throw new ArgumentException("No se ha proporcionado ningún archivo.");
        }

        const long maxFileSize = 5 * 1024 * 1024; // 5 MB
        if (file.Length > maxFileSize)
        {
            throw new ArgumentException("El archivo excede el tamaño máximo permitido de 5 MB.");
        }

        var allowedFormats = new[] { ".jpg", ".jpeg", ".png", ".webp" };
        var extension = Path.GetExtension(file.FileName).ToLowerInvariant();

        if (!file.ContentType.StartsWith("image/") || !allowedFormats.Contains(extension))
        {
            throw new ArgumentException("El archivo debe ser una imagen en formato jpg, jpeg, png o webp.");
        }

        await using var stream = file.OpenReadStream();
        var uploadParams = new ImageUploadParams
        {
            File = new FileDescription(file.FileName, stream),
            Folder = folder,
            Transformation = new Transformation().Width(500).Height(500).Crop("fill").Gravity("face")
        };

        try
        {
            var result = await _cloudinary.UploadAsync(uploadParams);
            return result;
        }
        catch (Exception ex)
        {
            throw new ApplicationException("Error subiendo imagen a Cloudinary", ex);
        }
    }

    public async Task<DeletionResult> DeletePhotoAsync(string publicId)
    {
        if (string.IsNullOrWhiteSpace(publicId))
            throw new ArgumentException("El publicId no puede estar vacío.");

        try
        {
            var deleteParams = new DeletionParams(publicId);
            var result = await _cloudinary.DestroyAsync(deleteParams);

            // Manejo cuando la imagen no existe
            if (result.Result == "not found")
            {
                // Podrías loguear esta situación o simplemente ignorarla
            }

            return result;
        }
        catch (Exception ex)
        {
            // Loggear o manejar error inesperado
            throw new ApplicationException($"Error al eliminar imagen con publicId {publicId}", ex);
        }
    }

}
