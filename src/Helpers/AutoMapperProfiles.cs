using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using AutoMapper;

using ECommerce.src.Dto;
using ECommerce.src.Models;

namespace ECommerce.src.Helpers
{
    /// <summary>
    /// AutoMapper profiles for mapping between domain models and DTOs.
    /// This class defines the mapping configuration for AutoMapper.
    /// </summary>
    public class AutoMapperProfiles : Profile
    {
        /// <summary>
        /// AutoMapperProfiles constructor.
        /// Initializes the mapping configuration for AutoMapper.
        /// </summary>
        public AutoMapperProfiles()
        {   
            // Create a mapping configuration for Product to GetProductDto
            CreateMap<Product, GetProductDto>()
                .ForMember(dest => dest.Category, opt => opt.MapFrom(src => src.Category))
                .ForMember(dest => dest.Brand, opt => opt.MapFrom(src => src.Brand))
                .ForMember(dest => dest.Condition, opt => opt.MapFrom(src => src.Condition));
        }
    }
}