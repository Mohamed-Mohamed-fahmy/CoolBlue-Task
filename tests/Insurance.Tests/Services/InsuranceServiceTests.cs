using Insurance.Api.Interfaces;
using Insurance.Api.DTOs;
using Insurance.Api.Services;
using Moq;
using System.Collections.Generic;
using Xunit;
using Microsoft.Extensions.Logging;
using System.Reflection;

namespace Insurance.Tests.Services
{
    public class InsuranceServiceTests
    {
        private readonly Mock<IProductDataService> productDataServiceMock;
        private readonly InsuranceService serviceUnderTest;
        private readonly Mock<ISurchargeService> surchargeServiceMock;
        private readonly Mock<ILogger<InsuranceService>> loggerMock;

        public InsuranceServiceTests()
        {
            this.productDataServiceMock = new Mock<IProductDataService>();
            this.surchargeServiceMock = new Mock<ISurchargeService>();
            this.loggerMock = new Mock<ILogger<InsuranceService>>();

            this.serviceUnderTest = new InsuranceService(this.productDataServiceMock.Object, this.surchargeServiceMock.Object, this.loggerMock.Object);
        }

        [Fact]
        public async void CalculateInsurance_GivenSalesPriceLessThan500AndProductTypeLaptop_ShouldAddFiveHundredEurosToInsuranceCost()
        {
            const float expectedInsuranceValue = 500;

            var productDto = new ProductDto
            {
                ProductId = 2,
                Name = "Test Product 2",
                ProductTypeId = 2,
                SalesPrice = 350
            };

            var productTypeDto = new ProductTypeDto
            {
                ProductTypeId = 2,
                Name = "Laptops",
                CanBeInsured = true
            };

            var productInsuranceDto = new ProductInsuranceDto
            {
                ProductId = productDto.ProductId
            };

            this.productDataServiceMock.Setup(productData => productData.GetProductDetails(It.IsAny<int>())).ReturnsAsync(productDto);
            this.productDataServiceMock.Setup(productData => productData.GetProductTypeDetails(It.IsAny<int>())).ReturnsAsync(productTypeDto);

            var result = await this.serviceUnderTest.CalculateInsurance(productInsuranceDto);

            Assert.Equal(
                expected: expectedInsuranceValue,
                actual: result
            );
        }

        [Fact]
        public async void CalculateInsurance_GivenSalesPriceLessThan500AndProductTypeNotLaptopOrSmartphone_ShouldNotAddInsuranceCost()
        {
            const float expectedInsuranceValue = 0;

            var productDto = new ProductDto
            {
                ProductId = 3,
                Name = "Test Product 3",
                ProductTypeId = 1,
                SalesPrice = 250
            };

            var productTypeDto = new ProductTypeDto
            {
                ProductTypeId = 1,
                Name = "Test type",
                CanBeInsured = true
            };

            var productInsuranceDto = new ProductInsuranceDto
            {
                ProductId = productDto.ProductId
            };

            this.productDataServiceMock.Setup(productData => productData.GetProductDetails(productDto.ProductId)).ReturnsAsync(productDto);
            this.productDataServiceMock.Setup(productData => productData.GetProductTypeDetails(productDto.ProductTypeId)).ReturnsAsync(productTypeDto);

            var result = await this.serviceUnderTest.CalculateInsurance(productInsuranceDto);

            Assert.Equal(
                expected: expectedInsuranceValue,
                actual: result
            );
        }

        [Fact]
        public async void CalculateInsurance_GivenSalesPriceBetween500And2000Euros_ShouldAddThousandEurosToInsuranceCost()
        {
            const float expectedInsuranceValue = 1000;

            var productDto = new ProductDto
            {
                ProductId = 1,
                Name = "Test Product",
                ProductTypeId = 1,
                SalesPrice = 750
            };

            var productTypeDto = new ProductTypeDto
            {
                ProductTypeId = 1,
                Name = "Test type",
                CanBeInsured = true
            };

            var productInsuranceDto = new ProductInsuranceDto
            {
                ProductId = productDto.ProductId
            };

            this.productDataServiceMock.Setup(productData => productData.GetProductDetails(productDto.ProductId)).ReturnsAsync(productDto);
            this.productDataServiceMock.Setup(productData => productData.GetProductTypeDetails(productDto.ProductTypeId)).ReturnsAsync(productTypeDto);

            var result = await this.serviceUnderTest.CalculateInsurance(productInsuranceDto);

            Assert.Equal(
                expected: expectedInsuranceValue,
                actual: result
            );
        }

        [Fact]
        public async void CalculateInsurance_GivenSalesPriceMoreThanOrEqual2000Euros_ShouldAddTwoThousandEurosToInsuranceCost()
        {
            const float expectedInsuranceValue = 2000;

            var productDto = new ProductDto
            {
                ProductId = 4,
                Name = "Test Product 4",
                ProductTypeId = 1,
                SalesPrice = 2000
            };

            var productTypeDto = new ProductTypeDto
            {
                ProductTypeId = 1,
                Name = "Test type",
                CanBeInsured = true
            };

            var productInsuranceDto = new ProductInsuranceDto
            {
                ProductId = productDto.ProductId
            };

            this.productDataServiceMock.Setup(productData => productData.GetProductDetails(productDto.ProductId)).ReturnsAsync(productDto);
            this.productDataServiceMock.Setup(productData => productData.GetProductTypeDetails(productDto.ProductTypeId)).ReturnsAsync(productTypeDto);

            var result = await this.serviceUnderTest.CalculateInsurance(productInsuranceDto);

            Assert.Equal(
                expected: expectedInsuranceValue,
                actual: result
            );
        }

        [Fact]
        public async void CalculateOrderInsurance_GivenTwoProductsWithSalesPriceLessThan500AndProductTypeNotLaptopOrSmartphone_ShouldNotAddInsuranceCost()
        {
            const float expectedInsuranceValue = 0;

            var productDto = new ProductDto
            {
                ProductId = 3,
                Name = "Test Product 3",
                ProductTypeId = 1,
                SalesPrice = 250
            };

            var productTypeDto = new ProductTypeDto
            {
                ProductTypeId = 1,
                Name = "Test type",
                CanBeInsured = true
            };

            var orderInsuranceDto = new OrderInsuranceDto
            {
                ProductIds = new List<int>
                {
                    productDto.ProductId,
                    productDto.ProductId
                }
            };

            var surchargeRateDto = new GetSurchargeRateDto
            {
                Error = "",
                IsSuccess = false
            };

            this.surchargeServiceMock.Setup(surcharge => surcharge.GetSurchargeRate(It.IsAny<int>())).Returns(surchargeRateDto);
            this.productDataServiceMock.Setup(productData => productData.GetProductDetails(productDto.ProductId)).ReturnsAsync(productDto);
            this.productDataServiceMock.Setup(productData => productData.GetProductTypeDetails(productDto.ProductTypeId)).ReturnsAsync(productTypeDto);

            var result = await this.serviceUnderTest.CalculateOrderInsurance(orderInsuranceDto);

            Assert.Equal(
                expected: expectedInsuranceValue,
                actual: result
            );
        }

        [Fact]
        public async void CalculateOrderInsurance_GivenTwoProductsWithSalesPriceBetween500And2000Euros_ShouldAddTwoThousandEurosToInsuranceCost()
        {
            var productDto = new ProductDto
            {
                ProductId = 1,
                Name = "Test Product",
                ProductTypeId = 1,
                SalesPrice = 750
            };

            var productTypeDto = new ProductTypeDto
            {
                ProductTypeId = 1,
                Name = "Test type",
                CanBeInsured = true
            };

            var orderInsuranceDto = new OrderInsuranceDto
            {
                ProductIds = new List<int>{
                    productDto.ProductId,
                    productDto.ProductId
                }
            };

            float expectedInsuranceValue = 1000 * orderInsuranceDto.ProductIds.Count;

            var surchargeRateDto = new GetSurchargeRateDto
            {
                Error = "",
                IsSuccess = false
            };

            this.surchargeServiceMock.Setup(surcharge => surcharge.GetSurchargeRate(It.IsAny<int>())).Returns(surchargeRateDto);
            this.productDataServiceMock.Setup(productData => productData.GetProductDetails(productDto.ProductId)).ReturnsAsync(productDto);
            this.productDataServiceMock.Setup(productData => productData.GetProductTypeDetails(productDto.ProductTypeId)).ReturnsAsync(productTypeDto);

            var result = await this.serviceUnderTest.CalculateOrderInsurance(orderInsuranceDto);

            Assert.Equal(
                expected: expectedInsuranceValue,
                actual: result
            );
        }

        [Fact]
        public async void CalculateOrderInsurance_GivenTwoProductsWithSalesPriceBetween500And2000EurosAndOneWithProductTypeDigitalCamera_ShouldAddTwoThousandAndFiveHundredEurosToInsuranceCost()
        {
            var productDto = new ProductDto
            {
                ProductId = 1,
                Name = "Test Product",
                ProductTypeId = 1,
                SalesPrice = 750
            };
            var secondProductDto = new ProductDto
            {
                ProductId = 2,
                Name = "Test Product 2",
                ProductTypeId = 2,
                SalesPrice = 750
            };

            var productTypeDto = new ProductTypeDto
            {
                ProductTypeId = 1,
                Name = "Test type",
                CanBeInsured = true
            };

            var secondProductTypeDto = new ProductTypeDto
            {
                ProductTypeId = 2,
                Name = "Digital cameras",
                CanBeInsured = true
            };

            var orderInsuranceDto = new OrderInsuranceDto
            {
                ProductIds = new List<int>{
                    productDto.ProductId,
                    secondProductDto.ProductId
                }
            };

            float digitalCameraInsuranceValue = 500;
            float expectedInsuranceValue = (1000 * orderInsuranceDto.ProductIds.Count) + digitalCameraInsuranceValue;

            var surchargeRateDto = new GetSurchargeRateDto
            {
                Error = "",
                IsSuccess = false
            };

            this.surchargeServiceMock.Setup(surcharge => surcharge.GetSurchargeRate(It.IsAny<int>())).Returns(surchargeRateDto);
            this.productDataServiceMock.Setup(productData => productData.GetProductDetails(productDto.ProductId)).ReturnsAsync(productDto);
            this.productDataServiceMock.Setup(productData => productData.GetProductTypeDetails(productDto.ProductTypeId)).ReturnsAsync(productTypeDto);
            this.productDataServiceMock.Setup(productData => productData.GetProductDetails(secondProductDto.ProductId)).ReturnsAsync(secondProductDto);
            this.productDataServiceMock.Setup(productData => productData.GetProductTypeDetails(secondProductTypeDto.ProductTypeId)).ReturnsAsync(secondProductTypeDto);

            var result = await this.serviceUnderTest.CalculateOrderInsurance(orderInsuranceDto);

            Assert.Equal(
                expected: expectedInsuranceValue,
                actual: result
            );
        }

        [Fact]
        public async void CalculateOrderInsurance_GivenThreeProductsWithSalesPriceLessThan500AndTwoWithProductTypeDigitalCamera_ShoulAddOnlyFiveHundredEurosToInsuranceCost()
        {
            const float expectedInsuranceValue = 500;

            var productDto = new ProductDto
            {
                ProductId = 1,
                Name = "Test Product 1",
                ProductTypeId = 2,
                SalesPrice = 250
            };

            var secondProductDto = new ProductDto
            {
                ProductId = 2,
                Name = "Test Product 2",
                ProductTypeId = 2,
                SalesPrice = 250
            };

            var thirdProductDto = new ProductDto
            {
                ProductId = 3,
                Name = "Test Product 3",
                ProductTypeId = 1,
                SalesPrice = 250
            };

            var productTypeDto = new ProductTypeDto
            {
                ProductTypeId = 1,
                Name = "Test type",
                CanBeInsured = true
            };

            var digitalCameraProductTypeDto = new ProductTypeDto
            {
                ProductTypeId = 2,
                Name = "Digital cameras",
                CanBeInsured = true
            };

            var orderInsuranceDto = new OrderInsuranceDto
            {
                ProductIds = new List<int>
                {
                    productDto.ProductId,
                    secondProductDto.ProductId,
                    thirdProductDto.ProductId
                }
            };

            var surchargeRateDto = new GetSurchargeRateDto
            {
                Error = "",
                IsSuccess = false
            };

            this.surchargeServiceMock.Setup(surcharge => surcharge.GetSurchargeRate(It.IsAny<int>())).Returns(surchargeRateDto);
            this.productDataServiceMock.Setup(productData => productData.GetProductDetails(productDto.ProductId)).ReturnsAsync(productDto);
            this.productDataServiceMock.Setup(productData => productData.GetProductTypeDetails(productDto.ProductTypeId)).ReturnsAsync(productTypeDto);
            this.productDataServiceMock.Setup(productData => productData.GetProductDetails(secondProductDto.ProductId)).ReturnsAsync(productDto);
            this.productDataServiceMock.Setup(productData => productData.GetProductTypeDetails(digitalCameraProductTypeDto.ProductTypeId)).ReturnsAsync(digitalCameraProductTypeDto);
            this.productDataServiceMock.Setup(productData => productData.GetProductDetails(thirdProductDto.ProductId)).ReturnsAsync(productDto);

            var result = await this.serviceUnderTest.CalculateOrderInsurance(orderInsuranceDto);

            Assert.Equal(
                expected: expectedInsuranceValue,
                actual: result
            );
        }

        [Fact]
        public async void CalculateOrderInsurance_GivenTwoProductsWithSalesPriceBetween500And2000EurosAndSurchargeRateTenPercent_ShouldAddTwoThousandAndTwoHundredEurosToInsuranceCost()
        {
            var productDto = new ProductDto
            {
                ProductId = 1,
                Name = "Test Product",
                ProductTypeId = 1,
                SalesPrice = 750
            };

            var productTypeDto = new ProductTypeDto
            {
                ProductTypeId = 1,
                Name = "Test type",
                CanBeInsured = true
            };

            var orderInsuranceDto = new OrderInsuranceDto
            {
                ProductIds = new List<int>{
                    productDto.ProductId,
                    productDto.ProductId
                }
            };

            var surchargeRateDto = new GetSurchargeRateDto
            {
                ProductTypeId = 1,
                SurchargeRate = 10,
                IsSuccess = true
            };
            var expectedSurchargeValue = (1000 * surchargeRateDto.SurchargeRate) / 100;

            float expectedInsuranceValue = (1000 + expectedSurchargeValue) * orderInsuranceDto.ProductIds.Count;

            this.surchargeServiceMock.Setup(surcharge => surcharge.GetSurchargeRate(productTypeDto.ProductTypeId)).Returns(surchargeRateDto);
            this.productDataServiceMock.Setup(productData => productData.GetProductDetails(productDto.ProductId)).ReturnsAsync(productDto);
            this.productDataServiceMock.Setup(productData => productData.GetProductTypeDetails(productDto.ProductTypeId)).ReturnsAsync(productTypeDto);

            var result = await this.serviceUnderTest.CalculateOrderInsurance(orderInsuranceDto);

            Assert.Equal(
                expected: expectedInsuranceValue,
                actual: result
            );
        }

        [Fact]
        public async void CalculateOrderInsurance_GivenTwoProductsWithSalesPriceBetween500And2000EurosAndNoSurchargeRate_ShouldAddTwoThousandEurosToInsuranceCost()
        {
            var productDto = new ProductDto
            {
                ProductId = 1,
                Name = "Test Product",
                ProductTypeId = 1,
                SalesPrice = 750
            };

            var productTypeDto = new ProductTypeDto
            {
                ProductTypeId = 1,
                Name = "Test type",
                CanBeInsured = true
            };

            var orderInsuranceDto = new OrderInsuranceDto
            {
                ProductIds = new List<int>{
                    productDto.ProductId,
                    productDto.ProductId
                }
            };

            var surchargeRateDto = new GetSurchargeRateDto
            {
                Error = "",
                IsSuccess = false
            };

            float expectedInsuranceValue = 1000 * orderInsuranceDto.ProductIds.Count;

            this.surchargeServiceMock.Setup(surcharge => surcharge.GetSurchargeRate(productTypeDto.ProductTypeId)).Returns(surchargeRateDto);
            this.productDataServiceMock.Setup(productData => productData.GetProductDetails(productDto.ProductId)).ReturnsAsync(productDto);
            this.productDataServiceMock.Setup(productData => productData.GetProductTypeDetails(productDto.ProductTypeId)).ReturnsAsync(productTypeDto);

            var result = await this.serviceUnderTest.CalculateOrderInsurance(orderInsuranceDto);

            Assert.Equal(
                expected: expectedInsuranceValue,
                actual: result
            );
        }

        [Fact]
        public void CalculateProductInsurance_GivenSalesPriceLessThan500AndProductTypeNotLaptopOrSmartphone_ShouldNotAddInsuranceCost()
        {
            const float expectedInsuranceValue = 0;

            var productDto = new ProductDto
            {
                ProductId = 3,
                Name = "Test Product 3",
                ProductTypeId = 1,
                SalesPrice = 250,
                ProductType= new ProductTypeDto
                {
                    ProductTypeId = 1,
                    Name = "Test type",
                    CanBeInsured = true
                }
            };
            MethodInfo methodInfo = typeof(InsuranceService).GetMethod("CalculateProductInsurance", BindingFlags.NonPublic | BindingFlags.Instance);
            object[] parameters = { productDto };
            var result = methodInfo.Invoke(this.serviceUnderTest, parameters);

            Assert.Equal(
                expected: expectedInsuranceValue,
                actual: result);
        }

        [Fact]
        public void CalculateProductInsurance_GivenProductTypeNotInsured_ShouldNotAddInsuranceCost()
        {
            const float expectedInsuranceValue = 0;

            var productDto = new ProductDto
            {
                ProductId = 3,
                Name = "Test Product 3",
                ProductTypeId = 1,
                SalesPrice = 700,
                ProductType = new ProductTypeDto
                {
                    ProductTypeId = 1,
                    Name = "Test type",
                    CanBeInsured = false
                }
            };
            MethodInfo methodInfo = typeof(InsuranceService).GetMethod("CalculateProductInsurance", BindingFlags.NonPublic | BindingFlags.Instance);
            object[] parameters = { productDto };
            var result = methodInfo.Invoke(this.serviceUnderTest, parameters);

            Assert.Equal(
                expected: expectedInsuranceValue,
                actual: result);
        }
    }
}
