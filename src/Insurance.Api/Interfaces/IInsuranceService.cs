using Insurance.Api.DTOs;
using System.Threading.Tasks;

namespace Insurance.Api.Interfaces
{
    public interface IInsuranceService
    {
        Task<float> CalculateInsurance(ProductInsuranceDto calculateInsuranceDto);
        Task<float> CalculateOrderInsurance(OrderInsuranceDto calculateOrderInsuranceDto);
    }
}
