using Insurance.Api.DTOs;
using Insurance.Api.Interfaces;
using Insurance.Api.Services;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace Insurance.Tests.Services
{
    public class ProductDataServiceTests
    {
        private readonly Mock<IHttpClientHandler> httpClientHandlerMock;
        private readonly Mock<ILogger<IProductDataService>> loggerMock;

        private readonly ProductDataService serviceUnderTest;

        public ProductDataServiceTests()
        {
            this.httpClientHandlerMock = new Mock<IHttpClientHandler>();
            this.loggerMock = new Mock<ILogger<IProductDataService>>();

            this.serviceUnderTest = new ProductDataService(this.httpClientHandlerMock.Object, this.loggerMock.Object);
        }

        [Fact]
        public async void GetProductDetails_SuccefullyReturnsProductDetails()
        {
            var productDto = new ProductDto
            {
                Name = "Test Product",
            };

            this.httpClientHandlerMock.Setup(clientHandler => clientHandler.GetResponse<ProductDto>(It.IsAny<string>())).ReturnsAsync(productDto);

            var result = await this.serviceUnderTest.GetProductDetails(1);

            Assert.Equal(
                expected: productDto.Name,
                actual: result.Name);
        }

        [Fact]
        public async void GetProductTypeDetails_SuccefullyReturnsProductTypeDetails()
        {
            var productDto = new ProductTypeDto
            {
                Name = "Test Product",
            };

            this.httpClientHandlerMock.Setup(clientHandler => clientHandler.GetResponse<ProductTypeDto>(It.IsAny<string>())).ReturnsAsync(productDto);

            var result = await this.serviceUnderTest.GetProductTypeDetails(1);

            Assert.Equal(
                expected: productDto.Name,
                actual: result.Name);
        }
    }
}
