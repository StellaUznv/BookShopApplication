using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BookShopApplication.Data.Migrations
{
    /// <inheritdoc />
    public partial class BookModelImagePathAndImageSeedingAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ImagePath",
                table: "Books",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: true,
                comment: "The image's path");

            migrationBuilder.UpdateData(
                table: "Books",
                keyColumn: "Id",
                keyValue: new Guid("11111111-2222-3333-4444-555555555555"),
                column: "ImagePath",
                value: "/images/books/FoundationBookCover.jpg");

            migrationBuilder.UpdateData(
                table: "Books",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"),
                column: "ImagePath",
                value: "/images/books/TheHobbitBookCover.jpg");

            migrationBuilder.UpdateData(
                table: "Books",
                keyColumn: "Id",
                keyValue: new Guid("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb"),
                column: "ImagePath",
                value: "/images/books/DuneBookCover.jpg");

            migrationBuilder.UpdateData(
                table: "Books",
                keyColumn: "Id",
                keyValue: new Guid("cccccccc-cccc-cccc-cccc-cccccccccccc"),
                column: "ImagePath",
                value: "/images/books/SherlockHolmesBookCover.jpg");

            migrationBuilder.UpdateData(
                table: "Books",
                keyColumn: "Id",
                keyValue: new Guid("dddddddd-dddd-dddd-dddd-dddddddddddd"),
                column: "ImagePath",
                value: "/images/books/Ender'sGameBookCover.jpg");

            migrationBuilder.UpdateData(
                table: "Books",
                keyColumn: "Id",
                keyValue: new Guid("eeeeeeee-eeee-eeee-eeee-eeeeeeeeeeee"),
                column: "ImagePath",
                value: "/images/books/TheNameOfTheWindBookCover.jpg");

            migrationBuilder.UpdateData(
                table: "Books",
                keyColumn: "Id",
                keyValue: new Guid("ffffffff-ffff-ffff-ffff-ffffffffffff"),
                column: "ImagePath",
                value: "/images/books/TheHoundOfTheBaskervillesBookCover.jpg");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImagePath",
                table: "Books");
        }
    }
}
