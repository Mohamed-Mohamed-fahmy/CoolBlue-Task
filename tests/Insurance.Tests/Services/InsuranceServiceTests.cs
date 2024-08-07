using Insurance.Api.Interfaces;
using Insurance.Api.Models;
using Insurance.Api.Services;
using Moq;
using System.Collections.Generic;
using Xunit;

namespace Insurance.Tests.Services
{
    public class InsuranceServiceTests
    {
        private readonly Mock<IBusinessRulesService> businessRulesServiceMock;
        public InsuranceServiceTests()
        {
            businessRulesServiceMock = new Mock<IBusinessRulesService>();
        }

        [Fact]
        public void CalculateInsurance_GivenSalesPriceLessThan500AndProductTypeLaptop_ShouldAddFiveHundredEurosToInsuranceCost()
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

            var calculateInsuranceDto = new CalculateInsuranceDto
            {
                ProductId = productDto.ProductId
            };

            this.businessRulesServiceMock.Setup(rules => rules.GetProductDetails(It.IsAny<int>())).Returns(productDto);
            this.businessRulesServiceMock.Setup(rules => rules.GetProductTypeDetails(It.IsAny<int>())).Returns(productTypeDto);

            var sut = new InsuranceService(this.businessRulesServiceMock.Object);

            var result = sut.CalculateInsurance(calculateInsuranceDto);

            Assert.Equal(
                expected: expectedInsuranceValue,
                actual: result
            );
        }

        [Fact]
        public void CalculateInsurance_GivenSalesPriceLessThan500AndProductTypeNotLaptopOrSmartphone_ShouldNotAddInsuranceCost()
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

            var calculateInsuranceDto = new CalculateInsuranceDto
            {
                ProductId = productDto.ProductId
            };

            this.businessRulesServiceMock.Setup(rules => rules.GetProductDetails(productDto.ProductId)).Returns(productDto);
            this.businessRulesServiceMock.Setup(rules => rules.GetProductTypeDetails(productDto.ProductTypeId)).Returns(productTypeDto);

            var sut = new InsuranceService(this.businessRulesServiceMock.Object);

            var result = sut.CalculateInsurance(calculateInsuranceDto);

            Assert.Equal(
                expected: expectedInsuranceValue,
                actual: result
            );
        }

        [Fact]
        public void CalculateInsurance_GivenSalesPriceBetween500And2000Euros_ShouldAddThousandEurosToInsuranceCost()
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

            var calculateInsuranceDto = new CalculateInsuranceDto
            {
                ProductId = productDto.ProductId
            };

            this.businessRulesServiceMock.Setup(rules => rules.GetProductDetails(productDto.ProductId)).Returns(productDto);
            this.businessRulesServiceMock.Setup(rules => rules.GetProductTypeDetails(productDto.ProductTypeId)).Returns(productTypeDto);

            var sut = new InsuranceService(this.businessRulesServiceMock.Object);

            var result = sut.CalculateInsurance(calculateInsuranceDto);

            Assert.Equal(
                expected: expectedInsuranceValue,
                actual: result
            );
        }

        [Fact]
        public void CalculateInsurance_GivenSalesPriceMoreThanOrEqual2000Euros_ShouldAddTwoThousandEurosToInsuranceCost()
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

            var calculateInsuranceDto = new CalculateInsuranceDto
            {
                ProductId = productDto.ProductId
            };

            this.businessRulesServiceMock.Setup(rules => rules.GetProductDetails(productDto.ProductId)).Returns(productDto);
            this.businessRulesServiceMock.Setup(rules => rules.GetProductTypeDetails(productDto.ProductTypeId)).Returns(productTypeDto);

            var sut = new InsuranceService(this.businessRulesServiceMock.Object);

            var result = sut.CalculateInsurance(calculateInsuranceDto);

            Assert.Equal(
                expected: expectedInsuranceValue,
                actual: result
            );
        }

        [Fact]
        public void CalculateOrderInsurance_GivenTwoProductsWithSalesPriceLessThan500AndProductTypeNotLaptopOrSmartphone_ShouldNotAddInsuranceCost()
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

            var calculateOrderInsuranceDto = new CalculateOrderInsuranceDto
            {
                ProductIds = new List<int>
                {
                    productDto.ProductId,
                    productDto.ProductId
                }
            };

            this.businessRulesServiceMock.Setup(rules => rules.GetProductDetails(productDto.ProductId)).Returns(productDto);
            this.businessRulesServiceMock.Setup(rules => rules.GetProductTypeDetails(productDto.ProductTypeId)).Returns(productTypeDto);

            var sut = new InsuranceService(this.businessRulesServiceMock.Object);

            var result = sut.CalculateOrderInsurance(calculateOrderInsuranceDto);

            Assert.Equal(
                expected: expectedInsuranceValue,
                actual: result
            );
        }

        [Fact]
        public void CalculateOrderInsurance_GivenTwoProductsWithSalesPriceBetween500And2000Euros_ShouldAddTwoThousandEurosToInsuranceCost()
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

            var calculateOrderInsuranceDto = new CalculateOrderInsuranceDto
            {
                ProductIds = new List<int>{
                    productDto.ProductId,
                    productDto.ProductId
                }
            };

            float expectedInsuranceValue = 1000 * calculateOrderInsuranceDto.ProductIds.Count;


            this.businessRulesServiceMock.Setup(rules => rules.GetProductDetails(productDto.ProductId)).Returns(productDto);
            this.businessRulesServiceMock.Setup(rules => rules.GetProductTypeDetails(productDto.ProductTypeId)).Returns(productTypeDto);

            var sut = new InsuranceService(this.businessRulesServiceMock.Object);

            var result = sut.CalculateOrderInsurance(calculateOrderInsuranceDto);

            Assert.Equal(
                expected: expectedInsuranceValue,
                actual: result
            );
        }
    }
}
