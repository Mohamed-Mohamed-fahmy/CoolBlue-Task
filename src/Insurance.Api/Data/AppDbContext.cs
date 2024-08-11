using Insurance.Api.Models;
using Microsoft.EntityFrameworkCore;
using System;

namespace Insurance.Api.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext()
        {

        }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }

        public virtual DbSet<ProductTypeSurcharge> ProductTypeSurcharges { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ProductTypeSurcharge>()
                .Property(p => p.Version)
                .IsConcurrencyToken();

            modelBuilder.Entity<ProductTypeSurcharge>().HasData(
                new ProductTypeSurcharge { Id = 1, ProductTypeId = 21, Version = Guid.NewGuid(), SurchargeRate = 10 },
                new ProductTypeSurcharge { Id = 2, ProductTypeId = 32, Version = Guid.NewGuid(), SurchargeRate = 20 },
                new ProductTypeSurcharge { Id = 3, ProductTypeId = 12, Version = Guid.NewGuid(), SurchargeRate = 30 });
        }
    }
}
