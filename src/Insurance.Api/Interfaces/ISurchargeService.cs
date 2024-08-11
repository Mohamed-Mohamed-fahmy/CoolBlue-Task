using Insurance.Api.DTOs;
using System.Threading.Tasks;

namespace Insurance.Api.Interfaces
{
    public interface ISurchargeService
    {
        Task<BaseResponse> UpdateSurchargeRate(SurchargeRateDto surchargeRateDto);

        GetSurchargeRateDto GetSurchargeRate(int productTypeId);
    }
}
