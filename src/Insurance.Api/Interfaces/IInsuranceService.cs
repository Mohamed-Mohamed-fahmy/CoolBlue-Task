using Insurance.Api.Models;

namespace Insurance.Api.Interfaces
{
    public interface IInsuranceService
    {
        float CalculateInsurance(CalculateInsuranceDto calculateInsuranceDto);
    }
}
