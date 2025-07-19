using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BookShopApplication.Data.Migrations
{
    /// <inheritdoc />
    public partial class LatitudeAndLongtitudeMovedToLocationEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Latitude",
                table: "Shops");

            migrationBuilder.DropColumn(
                name: "Longitude",
                table: "Shops");

            migrationBuilder.AddColumn<double>(
                name: "Latitude",
                table: "Locations",
                type: "float",
                nullable: false,
                defaultValue: 0.0,
                comment: "Latitude coordinate of the Shop");

            migrationBuilder.AddColumn<double>(
                name: "Longitude",
                table: "Locations",
                type: "float",
                nullable: false,
                defaultValue: 0.0,
                comment: "Longitude coordinate of the Shop");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("44444444-4444-4444-4444-444444444444"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "08323523-2771-412f-a129-9c0854056f65", "AQAAAAIAAYagAAAAEIli8dEu0QKxc7/GmO81VvJVUS8hTB3uYZ7KnkLNMms4uutg5L9Klj8v3tP0FTBFWw==", "8d46e217-78f3-4fdd-809e-1ff3a4f870c4" });

            migrationBuilder.UpdateData(
                table: "Locations",
                keyColumn: "Id",
                keyValue: new Guid("99999999-0000-0000-0000-000000000001"),
                columns: new[] { "Latitude", "Longitude" },
                values: new object[] { 40.712800000000001, -74.006 });

            migrationBuilder.UpdateData(
                table: "Locations",
                keyColumn: "Id",
                keyValue: new Guid("99999999-0000-0000-0000-000000000002"),
                columns: new[] { "Latitude", "Longitude" },
                values: new object[] { 51.507399999999997, -0.1278 });

            migrationBuilder.UpdateData(
                table: "Locations",
                keyColumn: "Id",
                keyValue: new Guid("99999999-0000-0000-0000-000000000003"),
                columns: new[] { "Latitude", "Longitude" },
                values: new object[] { 43.653199999999998, -79.383200000000002 });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Latitude",
                table: "Locations");

            migrationBuilder.DropColumn(
                name: "Longitude",
                table: "Locations");

            migrationBuilder.AddColumn<double>(
                name: "Latitude",
                table: "Shops",
                type: "float",
                nullable: false,
                defaultValue: 0.0,
                comment: "Latitude coordinate of the Shop");

            migrationBuilder.AddColumn<double>(
                name: "Longitude",
                table: "Shops",
                type: "float",
                nullable: false,
                defaultValue: 0.0,
                comment: "Longitude coordinate of the Shop");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("44444444-4444-4444-4444-444444444444"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "262206c2-4828-4a31-ba17-b542bc0a2951", "AQAAAAIAAYagAAAAEP2S+7mguUK11b5s3Mt8WUepmcklQt3L5Mt3MYLb/2xGvvnBiqqqxsWG8IuW0FNyXQ==", "47d627fd-2465-4025-9111-faef15877107" });

            migrationBuilder.UpdateData(
                table: "Shops",
                keyColumn: "Id",
                keyValue: new Guid("44444444-4444-4444-4444-444444444444"),
                columns: new[] { "Latitude", "Longitude" },
                values: new object[] { 40.712800000000001, -74.006 });

            migrationBuilder.UpdateData(
                table: "Shops",
                keyColumn: "Id",
                keyValue: new Guid("55555555-5555-5555-5555-555555555555"),
                columns: new[] { "Latitude", "Longitude" },
                values: new object[] { 51.507399999999997, -0.1278 });

            migrationBuilder.UpdateData(
                table: "Shops",
                keyColumn: "Id",
                keyValue: new Guid("66666666-6666-6666-6666-666666666666"),
                columns: new[] { "Latitude", "Longitude" },
                values: new object[] { 43.653199999999998, -79.383200000000002 });
        }
    }
}
