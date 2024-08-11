using AutoMapper;
using Insurance.Api.Data;
using Insurance.Api.DTOs;
using Insurance.Api.Helpers;
using Insurance.Api.Models;
using Insurance.Api.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using Xunit;

namespace Insurance.Tests.Services
{
    public class SurchargeServiceIntegrationTests
    {
        private readonly Mock<ILogger<SurchargeService>> loggerMock;
        private readonly IMapper mapper;
        private readonly DbContextOptions<AppDbContext> dbContextOptions;
        private SurchargeService serviceUnderTest;

        public SurchargeServiceIntegrationTests()
        {
            this.dbContextOptions = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: "InsuranceTestDb")
                .Options;

            var mappingConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new MappingProfile());
            });
            this.mapper = mappingConfig.CreateMapper();

            this.loggerMock = new Mock<ILogger<SurchargeService>>();
        }

        [Fact]
        public async void UpdateSurchargeRate_ConcurrencyConflict_ReturnsError()
        {
            using (var appDbContext = new AppDbContext(this.dbContextOptions))
            {
                appDbContext.Database.EnsureDeleted();

                var existingRate = new ProductTypeSurcharge { Id = 1, ProductTypeId = 1, SurchargeRate = 1, Version = Guid.NewGuid() };
                appDbContext.ProductTypeSurcharges.Add(existingRate);
                await appDbContext.SaveChangesAsync();

                this.serviceUnderTest = new SurchargeService(appDbContext, this.loggerMock.Object, this.mapper);

                var conflictingRate = new SurchargeRateDto { ProductTypeId = 1, SurchargeRate = 10, Version = Guid.NewGuid() };

                var result = await this.serviceUnderTest.UpdateSurchargeRate(conflictingRate);

                Assert.False(result.IsSuccess);

                Assert.Equal(
                    expected: "Concurrency conflict occurred.",
                    actual: result.Error);
            }
        }

        [Fact]
        public async void UpdateSurchargeRate_SurchargeRateNotFound_ReturnsError()
        {
            using (var appDbContext = new AppDbContext(this.dbContextOptions))
            {
                appDbContext.Database.EnsureDeleted();

                var expectedErrorMsg = "Surcharge rate not found";

                this.serviceUnderTest = new SurchargeService(appDbContext, this.loggerMock.Object, this.mapper);

                var surchargegRate = new SurchargeRateDto { ProductTypeId = 1, SurchargeRate = 10, Version = Guid.NewGuid() };

                var result = await this.serviceUnderTest.UpdateSurchargeRate(surchargegRate);

                Assert.False(result.IsSuccess);

                Assert.Equal(
                    expected: expectedErrorMsg,
                    actual: result.Error);
            }
        }

        [Fact]
        public async void UpdateSurchargeRate_SurchargeRate_UpdatesSurchargeRateSuccessfully()
        {
            using (var appDbContext = new AppDbContext(this.dbContextOptions))
            {
                appDbContext.Database.EnsureDeleted();

                var rowVersion = Guid.NewGuid();
                var existingRate = new ProductTypeSurcharge { Id = 1, ProductTypeId = 1, SurchargeRate = 20, Version = rowVersion };
                appDbContext.ProductTypeSurcharges.Add(existingRate);
                await appDbContext.SaveChangesAsync();

                this.serviceUnderTest = new SurchargeService(appDbContext, this.loggerMock.Object, this.mapper);

                var surchargegRate = new SurchargeRateDto { ProductTypeId = 1, SurchargeRate = 10, Version = rowVersion };

                var result = await this.serviceUnderTest.UpdateSurchargeRate(surchargegRate);

                Assert.True(result.IsSuccess);

                Assert.Equal(
                    expected: string.Empty,
                    actual: result.Error);
            }
        }

        [Fact]
        public void GetSurchargeRate_SurchargeRateNotFound_ReturnsError()
        {
            using (var appDbContext = new AppDbContext(this.dbContextOptions))
            {
                appDbContext.Database.EnsureDeleted();

                var expectedErrorMsg = "Surcharge rate not found";

                this.serviceUnderTest = new SurchargeService(appDbContext, this.loggerMock.Object, this.mapper);

                var result = this.serviceUnderTest.GetSurchargeRate(1);

                Assert.False(result.IsSuccess);
                Assert.Equal(
                    expected: expectedErrorMsg,
                    actual: result.Error);
            }
        }

        [Fact]
        public async void GetSurchargeRate_SurchargeRate_ReturnsSurchargeRateSuccessfully()
        {
            using (var appDbContext = new AppDbContext(this.dbContextOptions))
            {
                appDbContext.Database.EnsureDeleted();

                var rowVersion = Guid.NewGuid();
                var existingRate = new ProductTypeSurcharge { Id = 1, ProductTypeId = 1, SurchargeRate = 20, Version = rowVersion };
                appDbContext.ProductTypeSurcharges.Add(existingRate);
                await appDbContext.SaveChangesAsync();

                this.serviceUnderTest = new SurchargeService(appDbContext, this.loggerMock.Object, this.mapper);

                var result =  this.serviceUnderTest.GetSurchargeRate(1);

                Assert.True(result.IsSuccess);
                Assert.Equal(
                expected: existingRate.SurchargeRate,
                actual: result.SurchargeRate);
                Assert.Equal(
                    expected: existingRate.ProductTypeId,
                    actual: result.ProductTypeId);
            }
        }
    }
}
