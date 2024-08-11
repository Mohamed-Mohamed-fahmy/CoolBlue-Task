using Insurance.Api.Middlewares;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using Xunit;

namespace Insurance.Tests.Middlewares
{
    public class GlobalExceptionHandlerTests
    {
        private readonly Mock<ILogger<GlobalExceptionHandler>> loggerMock;

        public GlobalExceptionHandlerTests()
        {
            this.loggerMock= new Mock<ILogger<GlobalExceptionHandler>>();
        }

        [Fact]
        public async Task Middleware_Catches_ConflictException_Returns_ConflictResponse()
        {
            var middleware = new GlobalExceptionHandler(async (innerHttpContext) =>
            {
                throw new DbUpdateConcurrencyException("concurrency conflict occured.");
            }, this.loggerMock.Object);

            var context = new DefaultHttpContext();
            context.Response.Body = new MemoryStream();

            // Act
            await middleware.Invoke(context);

            // Assert
            context.Response.Body.Seek(0, SeekOrigin.Begin);
            var responseBody = await new StreamReader(context.Response.Body).ReadToEndAsync();
            var responseContent = JsonSerializer.Deserialize<Dictionary<string, string>>(responseBody);

            Assert.Equal(StatusCodes.Status409Conflict, context.Response.StatusCode);
            Assert.Equal("concurrency conflict occured.", responseContent["message"]);
        }

        [Fact]
        public async Task Middleware_Catches_GenericException_Returns_InternalServerError()
        {
            // Arrange
            var middleware = new GlobalExceptionHandler(async (innerHttpContext) =>
            {
                throw new Exception("Something went wrong.");
            }, this.loggerMock.Object);

            var context = new DefaultHttpContext();
            context.Response.Body = new MemoryStream();

            // Act
            await middleware.Invoke(context);

            // Assert
            context.Response.Body.Seek(0, SeekOrigin.Begin);
            var responseBody = await new StreamReader(context.Response.Body).ReadToEndAsync();
            var responseContent = JsonSerializer.Deserialize<Dictionary<string, string>>(responseBody);

            Assert.Equal(StatusCodes.Status500InternalServerError, context.Response.StatusCode);
            Assert.Equal("Something went wrong.", responseContent["message"]);
        }

        [Fact]
        public async Task InvokeAsync_CallsNextDelegate_WhenNoExceptionIsThrown()
        {
            // Arrange
            var nextMock = new Mock<RequestDelegate>();

            nextMock.Setup(_ => _(It.IsAny<HttpContext>())).Returns(Task.CompletedTask);

            var middleware = new GlobalExceptionHandler(nextMock.Object, loggerMock.Object);

            var context = new DefaultHttpContext();
            context.Response.Body = new MemoryStream();

            //Act
            await middleware.Invoke(context);

            //Assert
            nextMock.Verify(_ => _.Invoke(context), Times.Once);

            context.Response.Body.Seek(0, SeekOrigin.Begin);
            var responseBody = await new StreamReader(context.Response.Body).ReadToEndAsync();
            Assert.Equal(string.Empty, responseBody);
        }
    }
}
