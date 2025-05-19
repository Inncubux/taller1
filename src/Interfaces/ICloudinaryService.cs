using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using taller1.src.Dto.Product;

namespace taller1.src.Interfaces
{
    public interface ICloudinaryService
    {
        Task<ImageUploadResultDto> UploadImageAsync(IFormFile file, string folder);

        Task DeleteImageAsync(string publicId);
    }
}