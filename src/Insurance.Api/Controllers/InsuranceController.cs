using Insurance.Api.Interfaces;
using Insurance.Api.DTOs;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Insurance.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InsuranceController : ControllerBase
    {
        private readonly IInsuranceService insuranceService;
        public InsuranceController(IInsuranceService insuranceService)
        {
            this.insuranceService = insuranceService;
        }

        [HttpPost]
        [Route("productInsurance")]
        public async Task<IActionResult> CalculateProductInsurance([FromBody] CalculateInsuranceDto calculateInsuranceDto )
        {
            var insuranceValue = await this.insuranceService.CalculateInsurance(calculateInsuranceDto);
            return Ok(new { InsuranceValue = insuranceValue });
        }

        [HttpPost]
        [Route("orderInsurance")]
        public async Task<IActionResult> CalculateOrderInsurance([FromBody] CalculateOrderInsuranceDto calculateOrderInsuranceDto)
        {
            var insuranceValue = await this.insuranceService.CalculateOrderInsurance(calculateOrderInsuranceDto);
            return Ok(new { InsuranceValue = insuranceValue });
        }
    }
}
