using Application.DTO.Banner;
using Application.DTO.Partner;
using AutoMapper;
using Domain.Entities;
using e_commerce.core.Domain.Entities;
using e_commerce.core.DTO.Brand;

namespace Application.MapperConfig
{
    /// <summary>
    /// AutoMapper profile configuration
    /// </summary>
    public class MappConfig : Profile
    {
        public MappConfig()
        {
            // Brand mappings
            CreateMap<Brand, BrandResponceDto>().ReverseMap();

            // Banner mappings
            CreateMap<Banner, BannerResponseDto>().ReverseMap();

            // Partner mappings
            CreateMap<Partner, PartnerResponseDto>().ReverseMap();
        }
    }
}
