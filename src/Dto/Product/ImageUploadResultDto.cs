using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace taller1.src.Dto.Product
{
    public class ImageUploadResultDto
    {
        public string Url { get; set; } = string.Empty;
        public string PublicId { get; set; } = string.Empty;

        public static implicit operator string(ImageUploadResultDto v)
        {
            throw new NotImplementedException();
        }
    }
}