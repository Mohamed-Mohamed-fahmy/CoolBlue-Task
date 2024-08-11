using Insurance.Api.Interfaces;
using Insurance.Api.DTOs;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Insurance.Api.Controllers
{
    [Route("api/insurance")]
    [ApiController]
    public class InsuranceController : ControllerBase
    {
        private readonly IInsuranceService insuranceService;
        public InsuranceController(IInsuranceService insuranceService)
        {
            this.insuranceService = insuranceService;
        }

        [HttpPost]
        [Route("product")]
        public async Task<IActionResult> CalculateProductInsurance([FromBody] ProductInsuranceDto calculateInsuranceDto )
        {
            var insuranceValue = await this.insuranceService.CalculateInsurance(calculateInsuranceDto);
            return Ok(new { InsuranceValue = insuranceValue });
        }

        [HttpPost]
        [Route("order")]
        public async Task<IActionResult> CalculateOrderInsurance([FromBody] OrderInsuranceDto OrderInsuranceDto)
        {
            var insuranceValue = await this.insuranceService.CalculateOrderInsurance(OrderInsuranceDto);
            return Ok(new { InsuranceValue = insuranceValue });
        }
    }
}
