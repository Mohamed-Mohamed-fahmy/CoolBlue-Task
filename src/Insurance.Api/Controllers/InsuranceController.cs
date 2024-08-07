using Insurance.Api.Interfaces;
using Insurance.Api.Models;
using Microsoft.AspNetCore.Mvc;

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
        public IActionResult CalculateProductInsurance([FromBody] CalculateInsuranceDto calculateInsuranceDto )
        {
            var insuranceValue = this.insuranceService.CalculateInsurance(calculateInsuranceDto);
            return Ok(new { InsuranceValue = insuranceValue });
        }

        [HttpPost]
        [Route("orderInsurance")]
        public IActionResult CalculateOrderInsurance([FromBody] CalculateOrderInsuranceDto calculateOrderInsuranceDto)
        {
            var insuranceValue = this.insuranceService.CalculateOrderInsurance(calculateOrderInsuranceDto);
            return Ok(new { InsuranceValue = insuranceValue });
        }
    }
}
