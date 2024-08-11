using Insurance.Api.DTOs;
using System.Threading.Tasks;

namespace Insurance.Api.Interfaces
{
    public interface IInsuranceService
    {
        Task<float> CalculateInsurance(CalculateInsuranceDto calculateInsuranceDto);
        Task<float> CalculateOrderInsurance(CalculateOrderInsuranceDto calculateOrderInsuranceDto);
    }
}
