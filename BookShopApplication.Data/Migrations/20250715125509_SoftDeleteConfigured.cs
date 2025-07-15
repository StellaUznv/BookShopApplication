using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BookShopApplication.Data.Migrations
{
    /// <inheritdoc />
    public partial class SoftDeleteConfigured : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "CartItems",
                type: "bit",
                nullable: false,
                defaultValue: false,
                comment: "Tells if it's deleted or not");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "CartItems");
        }
    }
}
