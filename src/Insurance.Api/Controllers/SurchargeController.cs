using Insurance.Api.DTOs;
using Insurance.Api.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Insurance.Api.Controllers
{
    [Route("api/surcharge")]
    [ApiController]
    public class SurchargeController : ControllerBase
    {
        private readonly ISurchargeService surchargeService;

        public SurchargeController(ISurchargeService surchargeService)
        {
            this.surchargeService = surchargeService;
        }

        [HttpPost]
        [Route("update-rate")]
        public async Task<IActionResult> UpdateSurchargeRate([FromBody] SurchargeRateDto surchargeRateDto)
        {
            var response = await this.surchargeService.UpdateSurchargeRate(surchargeRateDto);

            if (response.IsSuccess)
            {
                return Ok(response);
            }

            return BadRequest(response);
        }
    }
}
