using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SDA.MK.CarParts.Migrations
{
    public partial class SeedParts : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Parts",
                columns: new[] { "Id", "Name", "Price" },
                values: new object[,]
                {
                    { new Guid("3f80271d-976f-476c-b888-9ecacc1d93e8"), "Spark plug", 60m },
                    { new Guid("415f4248-b2ef-4866-a3f8-7d926b11ca06"), "Brake pads", 85m },
                    { new Guid("67b95dbb-c0dd-4d37-bfda-cfc44e51e628"), "Brake disc", 200m },
                    { new Guid("73555f97-1548-42af-966e-91f48847e029"), "Oil filter", 40m },
                    { new Guid("88676fd8-276d-4915-aa73-bfd7b6a75584"), "Air filter", 15m },
                    { new Guid("947b2f45-537c-4230-b44e-bf729a4ae29a"), "Wiper blades", 70m },
                    { new Guid("a04ec53e-6602-447f-8957-a9391adc0250"), "Shock absorber", 135m },
                    { new Guid("a8fb81b2-57df-47e6-bb71-0ccff622ed53"), "Head gasket", 100m },
                    { new Guid("bdaf3a51-e4c3-449e-b3d1-b7c5ce7de391"), "Engine oil", 240m }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Parts",
                keyColumn: "Id",
                keyValue: new Guid("3f80271d-976f-476c-b888-9ecacc1d93e8"));

            migrationBuilder.DeleteData(
                table: "Parts",
                keyColumn: "Id",
                keyValue: new Guid("415f4248-b2ef-4866-a3f8-7d926b11ca06"));

            migrationBuilder.DeleteData(
                table: "Parts",
                keyColumn: "Id",
                keyValue: new Guid("67b95dbb-c0dd-4d37-bfda-cfc44e51e628"));

            migrationBuilder.DeleteData(
                table: "Parts",
                keyColumn: "Id",
                keyValue: new Guid("73555f97-1548-42af-966e-91f48847e029"));

            migrationBuilder.DeleteData(
                table: "Parts",
                keyColumn: "Id",
                keyValue: new Guid("88676fd8-276d-4915-aa73-bfd7b6a75584"));

            migrationBuilder.DeleteData(
                table: "Parts",
                keyColumn: "Id",
                keyValue: new Guid("947b2f45-537c-4230-b44e-bf729a4ae29a"));

            migrationBuilder.DeleteData(
                table: "Parts",
                keyColumn: "Id",
                keyValue: new Guid("a04ec53e-6602-447f-8957-a9391adc0250"));

            migrationBuilder.DeleteData(
                table: "Parts",
                keyColumn: "Id",
                keyValue: new Guid("a8fb81b2-57df-47e6-bb71-0ccff622ed53"));

            migrationBuilder.DeleteData(
                table: "Parts",
                keyColumn: "Id",
                keyValue: new Guid("bdaf3a51-e4c3-449e-b3d1-b7c5ce7de391"));
        }
    }
}
