using Insurance.Api.Controllers;
using Insurance.Api.DTOs;
using Insurance.Api.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Insurance.Tests.Controllers
{
    public class InsuranceControllerTests
    {
        private readonly Mock<IInsuranceService> inusranceServiceMock;
        private readonly InsuranceController controllerUnderTest;

        public InsuranceControllerTests()
        {
            this.inusranceServiceMock = new Mock<IInsuranceService>();

            this.controllerUnderTest = new InsuranceController(this.inusranceServiceMock.Object);
        }

        [Fact]
        public async void CalculateProductInsurance_SuccessfullyCalculateInsurance_ReturnsOk()
        {
            var expectedInsuranceValue = 1;

            var productInsuranceDto = new ProductInsuranceDto
            {
                ProductId = 1
            };

            this.inusranceServiceMock.Setup(insuranceService => insuranceService.CalculateInsurance(It.IsAny<ProductInsuranceDto>())).ReturnsAsync(1);

            var result = await this.controllerUnderTest.CalculateProductInsurance(productInsuranceDto);

            var okResult = Assert.IsType<OkObjectResult>(result);

            string resultBody = JsonConvert.SerializeObject(okResult.Value);
            var definition = new { InsuranceValue = "" };
            var anonymousResult = JsonConvert.DeserializeAnonymousType(resultBody, definition);


            Assert.Equal(
                expected: expectedInsuranceValue,
                actual: float.Parse(anonymousResult.InsuranceValue));
        }

        [Fact]
        public async void CalculateOrderInsurance_SuccessfullyCalculateInsurance_ReturnsOk()
        {
            var expectedInsuranceValue = 1;

            var prderInsuranceDto = new OrderInsuranceDto
            {
                ProductIds = new List<int> { 1 }
            };

            this.inusranceServiceMock.Setup(insuranceService => insuranceService.CalculateOrderInsurance(It.IsAny<OrderInsuranceDto>())).ReturnsAsync(1);

            var result = await this.controllerUnderTest.CalculateOrderInsurance(prderInsuranceDto);

            var okResult = Assert.IsType<OkObjectResult>(result);

            string resultBody = JsonConvert.SerializeObject(okResult.Value);
            var definition = new { InsuranceValue = "" };
            var anonymousResult = JsonConvert.DeserializeAnonymousType(resultBody, definition);


            Assert.Equal(
                expected: expectedInsuranceValue,
                actual: float.Parse(anonymousResult.InsuranceValue));
        }
    }
}
