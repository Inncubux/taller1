using AutoMapper;
using ECommerce.src.Dto.Product;
using ECommerce.src.Models;

using taller1.src.Dto.Product;
using taller1.src.Models.Relationship;

public class ProductProfile : Profile
{
    public ProductProfile()
    {
        CreateMap<ProductImage, ProductImageDto>();
        CreateMap<Product, GetProductDto>()
            .ForMember(dest => dest.Images, opt => opt.MapFrom(src => src.Images));
    }
}
