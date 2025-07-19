using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BookShopApplication.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddedAddressToLocationEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Address",
                table: "Locations",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: false,
                defaultValue: "",
                comment: "Address line");

            migrationBuilder.UpdateData(
                table: "Locations",
                keyColumn: "Id",
                keyValue: new Guid("99999999-0000-0000-0000-000000000001"),
                column: "Address",
                value: "123 Broadway Ave, Manhattan");

            migrationBuilder.UpdateData(
                table: "Locations",
                keyColumn: "Id",
                keyValue: new Guid("99999999-0000-0000-0000-000000000002"),
                column: "Address",
                value: "456 Brick Lane, Shoreditch");

            migrationBuilder.UpdateData(
                table: "Locations",
                keyColumn: "Id",
                keyValue: new Guid("99999999-0000-0000-0000-000000000003"),
                column: "Address",
                value: "789 King St W, Downtown");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Address",
                table: "Locations");
        }
    }
}
