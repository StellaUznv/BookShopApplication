using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace BookShopApplication.Data.Migrations
{
    /// <inheritdoc />
    public partial class SeedingDatabaseWithTheBasicEntities : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Genres",
                columns: new[] { "Id", "Description", "IsDeleted", "Name" },
                values: new object[,]
                {
                    { new Guid("11111111-1111-1111-1111-111111111111"), "Fantasy genre", false, "Fantasy" },
                    { new Guid("22222222-2222-2222-2222-222222222222"), "Science Fiction", false, "Sci-Fi" },
                    { new Guid("33333333-3333-3333-3333-333333333333"), "Mystery genre", false, "Mystery" }
                });

            migrationBuilder.InsertData(
                table: "Locations",
                columns: new[] { "Id", "CityName", "CountryName", "IsDeleted", "ZipCode" },
                values: new object[,]
                {
                    { new Guid("99999999-0000-0000-0000-000000000001"), "New York", "USA", false, "10001" },
                    { new Guid("99999999-0000-0000-0000-000000000002"), "London", "UK", false, "E1 6AN" },
                    { new Guid("99999999-0000-0000-0000-000000000003"), "Toronto", "Canada", false, "M5V" }
                });

            migrationBuilder.InsertData(
                table: "Books",
                columns: new[] { "Id", "AuthorName", "Description", "GenreId", "IsDeleted", "PagesNumber", "Price", "Title" },
                values: new object[,]
                {
                    { new Guid("11111111-2222-3333-4444-555555555555"), "Isaac Asimov", "Sci-fi foundation of a galactic empire", new Guid("22222222-2222-2222-2222-222222222222"), false, 296, 16.99m, "Foundation" },
                    { new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"), "Tolkien", "A fantasy book", new Guid("11111111-1111-1111-1111-111111111111"), false, 310, 15.99m, "The Hobbit" },
                    { new Guid("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb"), "Frank Herbert", "A sci-fi classic", new Guid("22222222-2222-2222-2222-222222222222"), false, 412, 19.99m, "Dune" },
                    { new Guid("cccccccc-cccc-cccc-cccc-cccccccccccc"), "Arthur Conan Doyle", "Mystery detective", new Guid("33333333-3333-3333-3333-333333333333"), false, 230, 12.99m, "Sherlock Holmes" },
                    { new Guid("dddddddd-dddd-dddd-dddd-dddddddddddd"), "Orson Scott Card", "Sci-fi military novel", new Guid("22222222-2222-2222-2222-222222222222"), false, 324, 14.99m, "Ender's Game" },
                    { new Guid("eeeeeeee-eeee-eeee-eeee-eeeeeeeeeeee"), "Patrick Rothfuss", "Fantasy epic", new Guid("11111111-1111-1111-1111-111111111111"), false, 662, 18.99m, "The Name of the Wind" },
                    { new Guid("ffffffff-ffff-ffff-ffff-ffffffffffff"), "Arthur Conan Doyle", "A thrilling mystery novel", new Guid("33333333-3333-3333-3333-333333333333"), false, 256, 10.99m, "The Hound of the Baskervilles" }
                });

            migrationBuilder.InsertData(
                table: "Shops",
                columns: new[] { "Id", "Description", "IsDeleted", "Latitude", "LocationId", "Longitude", "Name" },
                values: new object[,]
                {
                    { new Guid("44444444-4444-4444-4444-444444444444"), "Books in NY", false, 40.712800000000001, new Guid("99999999-0000-0000-0000-000000000001"), -74.006, "NY Bookstore" },
                    { new Guid("55555555-5555-5555-5555-555555555555"), "Books in London", false, 51.507399999999997, new Guid("99999999-0000-0000-0000-000000000002"), -0.1278, "London Reads" },
                    { new Guid("66666666-6666-6666-6666-666666666666"), "Books in Toronto", false, 43.653199999999998, new Guid("99999999-0000-0000-0000-000000000003"), -79.383200000000002, "Toronto Pages" }
                });

            migrationBuilder.InsertData(
                table: "BookInShops",
                columns: new[] { "BookId", "ShopId" },
                values: new object[,]
                {
                    { new Guid("11111111-2222-3333-4444-555555555555"), new Guid("55555555-5555-5555-5555-555555555555") },
                    { new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"), new Guid("44444444-4444-4444-4444-444444444444") },
                    { new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"), new Guid("66666666-6666-6666-6666-666666666666") },
                    { new Guid("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb"), new Guid("44444444-4444-4444-4444-444444444444") },
                    { new Guid("cccccccc-cccc-cccc-cccc-cccccccccccc"), new Guid("44444444-4444-4444-4444-444444444444") },
                    { new Guid("dddddddd-dddd-dddd-dddd-dddddddddddd"), new Guid("55555555-5555-5555-5555-555555555555") },
                    { new Guid("dddddddd-dddd-dddd-dddd-dddddddddddd"), new Guid("66666666-6666-6666-6666-666666666666") },
                    { new Guid("eeeeeeee-eeee-eeee-eeee-eeeeeeeeeeee"), new Guid("55555555-5555-5555-5555-555555555555") },
                    { new Guid("ffffffff-ffff-ffff-ffff-ffffffffffff"), new Guid("55555555-5555-5555-5555-555555555555") },
                    { new Guid("ffffffff-ffff-ffff-ffff-ffffffffffff"), new Guid("66666666-6666-6666-6666-666666666666") }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "BookInShops",
                keyColumns: new[] { "BookId", "ShopId" },
                keyValues: new object[] { new Guid("11111111-2222-3333-4444-555555555555"), new Guid("55555555-5555-5555-5555-555555555555") });

            migrationBuilder.DeleteData(
                table: "BookInShops",
                keyColumns: new[] { "BookId", "ShopId" },
                keyValues: new object[] { new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"), new Guid("44444444-4444-4444-4444-444444444444") });

            migrationBuilder.DeleteData(
                table: "BookInShops",
                keyColumns: new[] { "BookId", "ShopId" },
                keyValues: new object[] { new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"), new Guid("66666666-6666-6666-6666-666666666666") });

            migrationBuilder.DeleteData(
                table: "BookInShops",
                keyColumns: new[] { "BookId", "ShopId" },
                keyValues: new object[] { new Guid("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb"), new Guid("44444444-4444-4444-4444-444444444444") });

            migrationBuilder.DeleteData(
                table: "BookInShops",
                keyColumns: new[] { "BookId", "ShopId" },
                keyValues: new object[] { new Guid("cccccccc-cccc-cccc-cccc-cccccccccccc"), new Guid("44444444-4444-4444-4444-444444444444") });

            migrationBuilder.DeleteData(
                table: "BookInShops",
                keyColumns: new[] { "BookId", "ShopId" },
                keyValues: new object[] { new Guid("dddddddd-dddd-dddd-dddd-dddddddddddd"), new Guid("55555555-5555-5555-5555-555555555555") });

            migrationBuilder.DeleteData(
                table: "BookInShops",
                keyColumns: new[] { "BookId", "ShopId" },
                keyValues: new object[] { new Guid("dddddddd-dddd-dddd-dddd-dddddddddddd"), new Guid("66666666-6666-6666-6666-666666666666") });

            migrationBuilder.DeleteData(
                table: "BookInShops",
                keyColumns: new[] { "BookId", "ShopId" },
                keyValues: new object[] { new Guid("eeeeeeee-eeee-eeee-eeee-eeeeeeeeeeee"), new Guid("55555555-5555-5555-5555-555555555555") });

            migrationBuilder.DeleteData(
                table: "BookInShops",
                keyColumns: new[] { "BookId", "ShopId" },
                keyValues: new object[] { new Guid("ffffffff-ffff-ffff-ffff-ffffffffffff"), new Guid("55555555-5555-5555-5555-555555555555") });

            migrationBuilder.DeleteData(
                table: "BookInShops",
                keyColumns: new[] { "BookId", "ShopId" },
                keyValues: new object[] { new Guid("ffffffff-ffff-ffff-ffff-ffffffffffff"), new Guid("66666666-6666-6666-6666-666666666666") });

            migrationBuilder.DeleteData(
                table: "Books",
                keyColumn: "Id",
                keyValue: new Guid("11111111-2222-3333-4444-555555555555"));

            migrationBuilder.DeleteData(
                table: "Books",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"));

            migrationBuilder.DeleteData(
                table: "Books",
                keyColumn: "Id",
                keyValue: new Guid("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb"));

            migrationBuilder.DeleteData(
                table: "Books",
                keyColumn: "Id",
                keyValue: new Guid("cccccccc-cccc-cccc-cccc-cccccccccccc"));

            migrationBuilder.DeleteData(
                table: "Books",
                keyColumn: "Id",
                keyValue: new Guid("dddddddd-dddd-dddd-dddd-dddddddddddd"));

            migrationBuilder.DeleteData(
                table: "Books",
                keyColumn: "Id",
                keyValue: new Guid("eeeeeeee-eeee-eeee-eeee-eeeeeeeeeeee"));

            migrationBuilder.DeleteData(
                table: "Books",
                keyColumn: "Id",
                keyValue: new Guid("ffffffff-ffff-ffff-ffff-ffffffffffff"));

            migrationBuilder.DeleteData(
                table: "Shops",
                keyColumn: "Id",
                keyValue: new Guid("44444444-4444-4444-4444-444444444444"));

            migrationBuilder.DeleteData(
                table: "Shops",
                keyColumn: "Id",
                keyValue: new Guid("55555555-5555-5555-5555-555555555555"));

            migrationBuilder.DeleteData(
                table: "Shops",
                keyColumn: "Id",
                keyValue: new Guid("66666666-6666-6666-6666-666666666666"));

            migrationBuilder.DeleteData(
                table: "Genres",
                keyColumn: "Id",
                keyValue: new Guid("11111111-1111-1111-1111-111111111111"));

            migrationBuilder.DeleteData(
                table: "Genres",
                keyColumn: "Id",
                keyValue: new Guid("22222222-2222-2222-2222-222222222222"));

            migrationBuilder.DeleteData(
                table: "Genres",
                keyColumn: "Id",
                keyValue: new Guid("33333333-3333-3333-3333-333333333333"));

            migrationBuilder.DeleteData(
                table: "Locations",
                keyColumn: "Id",
                keyValue: new Guid("99999999-0000-0000-0000-000000000001"));

            migrationBuilder.DeleteData(
                table: "Locations",
                keyColumn: "Id",
                keyValue: new Guid("99999999-0000-0000-0000-000000000002"));

            migrationBuilder.DeleteData(
                table: "Locations",
                keyColumn: "Id",
                keyValue: new Guid("99999999-0000-0000-0000-000000000003"));
        }
    }
}
