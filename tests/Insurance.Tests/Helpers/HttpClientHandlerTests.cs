using Insurance.Api.DTOs;
using Microsoft.Extensions.Logging;
using Moq;
using Moq.Protected;
using Newtonsoft.Json;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Xunit;
using HttpClientHandler = Insurance.Api.Helpers.HttpClientHandler;

namespace Insurance.Tests.Helpers
{
    public class HttpClientHandlerTests
    {
        private readonly Mock<ILogger<HttpClientHandler>> loggerMock;
        private readonly Mock<IHttpClientFactory> httpClientFactoryMock;
        private readonly HttpClientHandler handlerUnderTest;
        public HttpClientHandlerTests()
        {
            this.loggerMock = new Mock<ILogger<HttpClientHandler>>();
            this.httpClientFactoryMock = new Mock<IHttpClientFactory>();

            this.handlerUnderTest = new HttpClientHandler(this.httpClientFactoryMock.Object, this.loggerMock.Object);
        }

        [Fact]
        public async Task GetResponse_ReturnsDeserializedObject_WhenResponseIsSuccessful()
        {
            // Arrange
            var expectedObject = new BaseResponse { IsSuccess = true };
            var responseContent = JsonConvert.SerializeObject(expectedObject);

            var mockHttpMessageHandler = new Mock<HttpMessageHandler>();
            mockHttpMessageHandler
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(responseContent),
                });

            var mockHttpClient = new HttpClient(mockHttpMessageHandler.Object);
            this.httpClientFactoryMock.Setup(httpClientFactory => httpClientFactory.CreateClient("ProductClient"))
                                  .Returns(mockHttpClient);

            // Act
            var result = await this.handlerUnderTest.GetResponse<BaseResponse>("http://test.com");

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedObject.IsSuccess, result.IsSuccess);
        }

        [Fact]
        public async Task GetResponse_LogsErrorAndThrowsException_WhenExceptionIsThrown()
        {
            // Arrange
            var exceptionMessage = "Network error";

            var mockHttpMessageHandler = new Mock<HttpMessageHandler>();
            mockHttpMessageHandler
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ThrowsAsync(new HttpRequestException(exceptionMessage));

            var mockHttpClient = new HttpClient(mockHttpMessageHandler.Object);
            this.httpClientFactoryMock.Setup(httpClientFactory => httpClientFactory.CreateClient("ProductClient"))
                                  .Returns(mockHttpClient);

            // Act & Assert
            var ex = await Assert.ThrowsAsync<HttpRequestException>(() =>
                this.handlerUnderTest.GetResponse<BaseResponse>("http://test.com"));

            Assert.Equal(exceptionMessage, ex.Message);
        }
    }
}
