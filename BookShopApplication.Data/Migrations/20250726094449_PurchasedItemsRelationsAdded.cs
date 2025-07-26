using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BookShopApplication.Data.Migrations
{
    /// <inheritdoc />
    public partial class PurchasedItemsRelationsAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsPurchased",
                table: "CartItems");

            migrationBuilder.CreateTable(
                name: "PurchaseItems",
                columns: table => new
                {
                    CartItemId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PurchaseItems", x => new { x.UserId, x.CartItemId });
                    table.ForeignKey(
                        name: "FK_PurchaseItems_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PurchaseItems_CartItems_CartItemId",
                        column: x => x.CartItemId,
                        principalTable: "CartItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("44444444-4444-4444-4444-444444444444"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "0df85b04-0d74-41c0-9912-9f1ed699fc9f", "AQAAAAIAAYagAAAAEI+v5C2cjmTsvSmMyqXud/tYX7Fand7gs+cyiD2jZl/yAJK9IiGgF1O3I7djBhXgAA==", "b2d5f250-3cdb-4459-80a2-ccc85d975d36" });

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseItems_CartItemId",
                table: "PurchaseItems",
                column: "CartItemId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PurchaseItems");

            migrationBuilder.AddColumn<bool>(
                name: "IsPurchased",
                table: "CartItems",
                type: "bit",
                nullable: false,
                defaultValue: false,
                comment: "Tells if it's bought or not");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("44444444-4444-4444-4444-444444444444"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "37747e0c-8df9-4a7d-b489-8b5365e087c6", "AQAAAAIAAYagAAAAENXeol8GqidjdD0ecFoRIrY/vFwrbDXGUVTyw6we3vW0Qy75HJ74qDIQMhY0kckPoQ==", "38591e48-c4b7-4dc2-b515-e11e8b99dd55" });
        }
    }
}
