using Insurance.Api.Controllers;
using Insurance.Api.DTOs;
using Insurance.Api.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using Xunit;

namespace Insurance.Tests.Controllers
{
    public class SurchargeControllerTests
    {
        private readonly Mock<ISurchargeService> surchargeServiceMock;
        private readonly SurchargeController controllerUnderTest;

        public SurchargeControllerTests()
        {
            this.surchargeServiceMock = new Mock<ISurchargeService>();
            this.controllerUnderTest = new SurchargeController(this.surchargeServiceMock.Object);
        }

        [Fact]
        public async void UpdateSurchargeRate_SurchargeRateNotFound_ReturnsBadRequest()
        {
            var notFoundResponse = new BaseResponse
            {
                Error = "Surcharge rate not found",
                IsSuccess = false
            };

            var surchargeRateDto = new SurchargeRateDto
            {
                ProductTypeId = 1,
                SurchargeRate = 10,
                Version = Guid.NewGuid()
            };

            this.surchargeServiceMock.Setup(surchargeService => surchargeService.UpdateSurchargeRate(It.IsAny<SurchargeRateDto>())).ReturnsAsync(notFoundResponse);

            var result = await this.controllerUnderTest.UpdateSurchargeRate(surchargeRateDto);

            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            var response = Assert.IsType<BaseResponse>(badRequestResult.Value);

            Assert.False(response.IsSuccess);
            Assert.Equal(
                expected: "Surcharge rate not found",
                actual: response.Error);

        }

        [Fact]
        public async void UpdateSurchargeRate_SurchargeRateConflict_ReturnsBadRequest()
        {
            var conflictResponse = new BaseResponse
            {
                Error = "Concurrency conflict occurred.",
                IsSuccess = false
            };

            var surchargeRateDto = new SurchargeRateDto
            {
                ProductTypeId = 1,
                SurchargeRate = 10,
                Version = Guid.NewGuid()
            };

            this.surchargeServiceMock.Setup(surchargeService => surchargeService.UpdateSurchargeRate(It.IsAny<SurchargeRateDto>())).ReturnsAsync(conflictResponse);

            var result = await this.controllerUnderTest.UpdateSurchargeRate(surchargeRateDto);

            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            var response = Assert.IsType<BaseResponse>(badRequestResult.Value);

            Assert.False(response.IsSuccess);
            Assert.Equal(
                expected: "Concurrency conflict occurred.",
                actual: response.Error);

        }

        [Fact]
        public async void UpdateSurchargeRate_SuccessfullyUpdatesSurchargeRate_ReturnsOk()
        {
            var successResponse = new BaseResponse
            {
                Error = string.Empty,
                IsSuccess = true
            }; 

            var surchargeRateDto = new SurchargeRateDto
            {
                ProductTypeId = 1,
                SurchargeRate = 10,
                Version = Guid.NewGuid()
            };

            this.surchargeServiceMock.Setup(surchargeService => surchargeService.UpdateSurchargeRate(It.IsAny<SurchargeRateDto>())).ReturnsAsync(successResponse);

            var result = await this.controllerUnderTest.UpdateSurchargeRate(surchargeRateDto);

            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsType<BaseResponse>(okResult.Value);

            Assert.True(response.IsSuccess);
            Assert.Equal(
                expected: string.Empty,
                actual: response.Error);

        }
    }
}
