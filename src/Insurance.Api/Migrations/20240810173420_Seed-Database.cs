using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Insurance.Api.Migrations
{
    /// <inheritdoc />
    public partial class SeedDatabase : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "ProductTypeSurcharges",
                columns: new[] { "Id", "ProductTypeId", "SurchargeRate", "Version" },
                values: new object[,]
                {
                    { 1, 21, 10m, new Guid("869040e7-3f1a-441c-b72b-d9f4c14f1ba6") },
                    { 2, 32, 20m, new Guid("26d5a2f3-c354-493e-af50-974f50908e0c") },
                    { 3, 12, 30m, new Guid("93acae4e-e962-4ba7-a168-e9b8c09d9ea0") }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "ProductTypeSurcharges",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "ProductTypeSurcharges",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "ProductTypeSurcharges",
                keyColumn: "Id",
                keyValue: 3);
        }
    }
}
