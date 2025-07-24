using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BookShopApplication.Data.Migrations
{
    /// <inheritdoc />
    public partial class BookInShopMageISoftDeletable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "BookInShops",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("44444444-4444-4444-4444-444444444444"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "37747e0c-8df9-4a7d-b489-8b5365e087c6", "AQAAAAIAAYagAAAAENXeol8GqidjdD0ecFoRIrY/vFwrbDXGUVTyw6we3vW0Qy75HJ74qDIQMhY0kckPoQ==", "38591e48-c4b7-4dc2-b515-e11e8b99dd55" });

            migrationBuilder.UpdateData(
                table: "BookInShops",
                keyColumns: new[] { "BookId", "ShopId" },
                keyValues: new object[] { new Guid("11111111-2222-3333-4444-555555555555"), new Guid("55555555-5555-5555-5555-555555555555") },
                column: "IsDeleted",
                value: false);

            migrationBuilder.UpdateData(
                table: "BookInShops",
                keyColumns: new[] { "BookId", "ShopId" },
                keyValues: new object[] { new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"), new Guid("44444444-4444-4444-4444-444444444444") },
                column: "IsDeleted",
                value: false);

            migrationBuilder.UpdateData(
                table: "BookInShops",
                keyColumns: new[] { "BookId", "ShopId" },
                keyValues: new object[] { new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"), new Guid("66666666-6666-6666-6666-666666666666") },
                column: "IsDeleted",
                value: false);

            migrationBuilder.UpdateData(
                table: "BookInShops",
                keyColumns: new[] { "BookId", "ShopId" },
                keyValues: new object[] { new Guid("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb"), new Guid("44444444-4444-4444-4444-444444444444") },
                column: "IsDeleted",
                value: false);

            migrationBuilder.UpdateData(
                table: "BookInShops",
                keyColumns: new[] { "BookId", "ShopId" },
                keyValues: new object[] { new Guid("cccccccc-cccc-cccc-cccc-cccccccccccc"), new Guid("44444444-4444-4444-4444-444444444444") },
                column: "IsDeleted",
                value: false);

            migrationBuilder.UpdateData(
                table: "BookInShops",
                keyColumns: new[] { "BookId", "ShopId" },
                keyValues: new object[] { new Guid("dddddddd-dddd-dddd-dddd-dddddddddddd"), new Guid("55555555-5555-5555-5555-555555555555") },
                column: "IsDeleted",
                value: false);

            migrationBuilder.UpdateData(
                table: "BookInShops",
                keyColumns: new[] { "BookId", "ShopId" },
                keyValues: new object[] { new Guid("dddddddd-dddd-dddd-dddd-dddddddddddd"), new Guid("66666666-6666-6666-6666-666666666666") },
                column: "IsDeleted",
                value: false);

            migrationBuilder.UpdateData(
                table: "BookInShops",
                keyColumns: new[] { "BookId", "ShopId" },
                keyValues: new object[] { new Guid("eeeeeeee-eeee-eeee-eeee-eeeeeeeeeeee"), new Guid("55555555-5555-5555-5555-555555555555") },
                column: "IsDeleted",
                value: false);

            migrationBuilder.UpdateData(
                table: "BookInShops",
                keyColumns: new[] { "BookId", "ShopId" },
                keyValues: new object[] { new Guid("ffffffff-ffff-ffff-ffff-ffffffffffff"), new Guid("55555555-5555-5555-5555-555555555555") },
                column: "IsDeleted",
                value: false);

            migrationBuilder.UpdateData(
                table: "BookInShops",
                keyColumns: new[] { "BookId", "ShopId" },
                keyValues: new object[] { new Guid("ffffffff-ffff-ffff-ffff-ffffffffffff"), new Guid("66666666-6666-6666-6666-666666666666") },
                column: "IsDeleted",
                value: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "BookInShops");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("44444444-4444-4444-4444-444444444444"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "08323523-2771-412f-a129-9c0854056f65", "AQAAAAIAAYagAAAAEIli8dEu0QKxc7/GmO81VvJVUS8hTB3uYZ7KnkLNMms4uutg5L9Klj8v3tP0FTBFWw==", "8d46e217-78f3-4fdd-809e-1ff3a4f870c4" });
        }
    }
}
