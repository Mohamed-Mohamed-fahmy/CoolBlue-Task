using AutoMapper;
using Insurance.Api.Data;
using Insurance.Api.DTOs;
using Insurance.Api.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Insurance.Api.Services
{
    public class SurchargeService : ISurchargeService
    {
        private readonly ILogger<SurchargeService> logger;
        private readonly AppDbContext appDbContext;
        private readonly IMapper mapper;

        public SurchargeService(AppDbContext appDbContext, ILogger<SurchargeService> logger, IMapper mapper)
        {
            this.appDbContext = appDbContext;
            this.logger = logger;
            this.mapper = mapper;
        }

        public async Task<BaseResponse> UpdateSurchargeRate(SurchargeRateDto surchargeRateDto)
        {
            var response = new BaseResponse();

            try
            {

                var currentRate = this.appDbContext.ProductTypeSurcharges.FirstOrDefault(rate => rate.ProductTypeId == surchargeRateDto.ProductTypeId);

                if (currentRate == null)
                {
                    this.logger.LogWarning($"Surcharge rate for ProductTypeId : {surchargeRateDto.ProductTypeId} is not found");

                    response.Error = "Surcharge rate not found";
                    response.IsSuccess = false;
                    return response;
                }

                if (currentRate.Version != surchargeRateDto.Version)
                {
                    this.logger.LogError($"Conflict occurred while updating product type for ProductTypeId : {surchargeRateDto.ProductTypeId}");

                    response.Error = "Concurrency conflict occurred.";
                    response.IsSuccess = false;
                    return response;
                }

                currentRate.SurchargeRate = surchargeRateDto.SurchargeRate;
                currentRate.Version = Guid.NewGuid();

                await this.appDbContext.SaveChangesAsync();
                this.logger.LogInformation($"Product type surcharge rate for ProductTypeId : {surchargeRateDto.ProductTypeId} is updated successfully");

                response.Error = string.Empty;
                response.IsSuccess = true;

            }
            catch (DbUpdateConcurrencyException ex)
            {
                this.logger.LogError($"Conflict detected while updating surcharge rate for ProductTypeId : {surchargeRateDto.ProductTypeId}");

                response.Error = "Concurrency conflict occurred.";
                response.IsSuccess = false;
                throw;
            }
            return response;
        }

        public GetSurchargeRateDto GetSurchargeRate(int productTypeId)
        {
            var response = new GetSurchargeRateDto();

            var currentRate = this.appDbContext.ProductTypeSurcharges.FirstOrDefault(rate => rate.ProductTypeId == productTypeId);

            if (currentRate == null)
            {
                this.logger.LogWarning($"Surcharge rate for ProductTypeId : {productTypeId} is not found");
                response.Error = "Surcharge rate not found";
                response.IsSuccess = false;

                return response;
            }

            response = this.mapper.Map<GetSurchargeRateDto>(currentRate);
            response.IsSuccess = true;

            this.logger.LogInformation($"Product type surcharge rate for ProductTypeId : {productTypeId} is retrieved successfully");

            return response;
        }
    }
}
