using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ECommerceAPI.Migrations
{
    /// <inheritdoc />
    public partial class AddGroupUniqueKeyOnProductColorSize : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Carts_UserId_ProductId",
                table: "Carts");

            migrationBuilder.CreateIndex(
                name: "IX_Carts_UserId_ProductId_SizeId_ColorId",
                table: "Carts",
                columns: new[] { "UserId", "ProductId", "SizeId", "ColorId" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Carts_UserId_ProductId_SizeId_ColorId",
                table: "Carts");

            migrationBuilder.CreateIndex(
                name: "IX_Carts_UserId_ProductId",
                table: "Carts",
                columns: new[] { "UserId", "ProductId" },
                unique: true);
        }
    }
}
