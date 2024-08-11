using AutoMapper;
using Insurance.Api.DTOs;
using Insurance.Api.Models;

namespace Insurance.Api.Helpers
{
    public class MappingProfile : Profile
    {
        public MappingProfile() 
        {
            CreateMap<ProductTypeSurcharge, SurchargeRateDto>();
            CreateMap<ProductTypeSurcharge, GetSurchargeRateDto>()
                .ForMember(dest => dest.ProductTypeId, act => act.MapFrom(src => src.ProductTypeId))
                .ForMember(dest => dest.SurchargeRate, act => act.MapFrom(src => src.SurchargeRate));
        }
    }
}
