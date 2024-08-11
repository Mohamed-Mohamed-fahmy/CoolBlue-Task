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
            CreateMap<ProductTypeSurcharge, GetSurchargeRateDto>();
        }
    }
}
