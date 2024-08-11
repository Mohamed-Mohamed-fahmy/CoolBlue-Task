using AutoMapper;
using Insurance.Api.Data;
using Insurance.Api.DTOs;
using Insurance.Api.Models;
using Insurance.Api.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Insurance.Tests.Services
{
    public class SurchargeServiceTests
    {
        private readonly Mock<ILogger<SurchargeService>> loggerMock;
        private readonly Mock<IMapper> mapperMock;
        private readonly Mock<AppDbContext> dbContextMock;
        private readonly SurchargeService serviceUnderTest;

        public SurchargeServiceTests()
        {
            this.loggerMock = new Mock<ILogger<SurchargeService>>();
            this.mapperMock = new Mock<IMapper>();
            this.dbContextMock = new Mock<AppDbContext>();

            this.serviceUnderTest = new SurchargeService(this.dbContextMock.Object, this.loggerMock.Object, this.mapperMock.Object);
        }

        [Fact]
        public async void UpdateSurchargeRate_SurchargeRateNotFound_ReturnsError()
        {
            var expectedErrorMsg = "Surcharge rate not found";

            var data = new List<ProductTypeSurcharge>().AsQueryable();
            var setMock = this.Create<ProductTypeSurcharge>(data);

            this.dbContextMock.Setup(c => c.ProductTypeSurcharges).Returns(setMock.Object);

            var surchargeRateDto = new SurchargeRateDto
            {
                ProductTypeId = 1,
                SurchargeRate = 10,
                Version = Guid.NewGuid()
            };

            var result = await this.serviceUnderTest.UpdateSurchargeRate(surchargeRateDto);

            Assert.False(result.IsSuccess);
            Assert.Equal(
                expected: expectedErrorMsg,
                actual: result.Error);
        }

        [Fact]
        public async void UpdateSurchargeRate_ConcurrencyConflict_ReturnsError()
        {
            var expectedErrorMsg = "Concurrency conflict occurred.";

            var surchargeRateDto = new SurchargeRateDto
            {
                ProductTypeId = 1,
                SurchargeRate = 10,
                Version = Guid.NewGuid()
            };

            var productTypeSurcharge = new List<ProductTypeSurcharge>
            {
                new ProductTypeSurcharge
                {
                    Id = 1,
                    ProductTypeId = 1,
                    SurchargeRate = 10,
                    Version = Guid.NewGuid()
                }
            }.AsQueryable();

            var setMock = this.Create<ProductTypeSurcharge>(productTypeSurcharge);

            this.dbContextMock.Setup(c => c.ProductTypeSurcharges).Returns(setMock.Object);

            var result = await this.serviceUnderTest.UpdateSurchargeRate(surchargeRateDto);

            Assert.False(result.IsSuccess);
            Assert.Equal(
                expected: expectedErrorMsg,
                actual: result.Error);
        }

        [Fact]
        public async void UpdateSurchargeRate_UpdatesSurchargeRateSuccessfully()
        {
            var rowVersion = Guid.NewGuid();
            var surchargeRateDto = new SurchargeRateDto
            {
                ProductTypeId = 1,
                SurchargeRate = 10,
                Version = rowVersion
            };

            var productTypeSurcharge = new List<ProductTypeSurcharge>
            {
                new ProductTypeSurcharge
                {
                    Id = 1,
                    ProductTypeId = 1,
                    SurchargeRate = 20,
                    Version = rowVersion
                }
            }.AsQueryable();

            var setMock = this.Create<ProductTypeSurcharge>(productTypeSurcharge);

            this.dbContextMock.Setup(c => c.ProductTypeSurcharges).Returns(setMock.Object);

            this.dbContextMock.Setup(db => db.SaveChangesAsync(It.IsAny<System.Threading.CancellationToken>()))
                          .ReturnsAsync(1);

            var result = await this.serviceUnderTest.UpdateSurchargeRate(surchargeRateDto);

            Assert.True(result.IsSuccess);
        }

        [Fact]
        public void GetSurchargeRate_SurchargeRateNotFound_ReturnsError()
        {
            var expectedErrorMsg = "Surcharge rate not found";

            var surchargeRateDto = new SurchargeRateDto
            {
                ProductTypeId = 1,
                SurchargeRate = 10,
                Version = Guid.NewGuid()
            };

            var surchargeRate = new List<ProductTypeSurcharge>().AsQueryable();
            var setMock = this.Create<ProductTypeSurcharge>(surchargeRate);

            this.dbContextMock.Setup(c => c.ProductTypeSurcharges).Returns(setMock.Object);

            var result = this.serviceUnderTest.GetSurchargeRate(1);

            Assert.False(result.IsSuccess);
            Assert.Equal(
                expected: expectedErrorMsg,
                actual: result.Error);
        }

        [Fact]
        public void GetSurchargeRate_SurchargeRate_ReturnsSurchargeRateSuccessfully()
        {
            var surchargeRateDto = new GetSurchargeRateDto
            {
                ProductTypeId = 1,
                SurchargeRate = 10,
            };

            var productTypeSurcharge = new List<ProductTypeSurcharge>
            {
                new ProductTypeSurcharge
                {
                    Id = 1,
                    ProductTypeId = 1,
                    SurchargeRate = 10,
                }
            }.AsQueryable();

            var setMock = this.Create<ProductTypeSurcharge>(productTypeSurcharge);

            this.dbContextMock.Setup(c => c.ProductTypeSurcharges).Returns(setMock.Object);
            this.mapperMock.Setup(mapper => mapper.Map<GetSurchargeRateDto>(It.IsAny<ProductTypeSurcharge>())).Returns(surchargeRateDto);

            var result = this.serviceUnderTest.GetSurchargeRate(1);

            Assert.True(result.IsSuccess);
            Assert.Equal(
                expected: surchargeRateDto.SurchargeRate,
                actual: result.SurchargeRate);
            Assert.Equal(
                expected: surchargeRateDto.ProductTypeId,
                actual: result.ProductTypeId);
        }

        private Mock<DbSet<T>> Create<T>(IEnumerable<T> data) where T : class
        {
            var queryable = data.AsQueryable();
            var setMock = new Mock<DbSet<T>>();

            setMock.As<IQueryable<T>>().Setup(m => m.Provider).Returns(queryable.Provider);
            setMock.As<IQueryable<T>>().Setup(m => m.Expression).Returns(queryable.Expression);
            setMock.As<IQueryable<T>>().Setup(m => m.ElementType).Returns(queryable.ElementType);
            setMock.As<IQueryable<T>>().Setup(m => m.GetEnumerator()).Returns(() => queryable.GetEnumerator());

            return setMock;
        }
    }
}
