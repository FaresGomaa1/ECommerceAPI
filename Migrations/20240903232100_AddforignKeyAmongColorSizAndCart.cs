using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ECommerceAPI.Migrations
{
    /// <inheritdoc />
    public partial class AddforignKeyAmongColorSizAndCart : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Color",
                table: "Carts");

            migrationBuilder.DropColumn(
                name: "Size",
                table: "Carts");

            migrationBuilder.AddColumn<int>(
                name: "ColorId",
                table: "Carts",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "SizeId",
                table: "Carts",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Carts_ColorId",
                table: "Carts",
                column: "ColorId");

            migrationBuilder.CreateIndex(
                name: "IX_Carts_SizeId",
                table: "Carts",
                column: "SizeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Carts_Colors_ColorId",
                table: "Carts",
                column: "ColorId",
                principalTable: "Colors",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Carts_Sizes_SizeId",
                table: "Carts",
                column: "SizeId",
                principalTable: "Sizes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Carts_Colors_ColorId",
                table: "Carts");

            migrationBuilder.DropForeignKey(
                name: "FK_Carts_Sizes_SizeId",
                table: "Carts");

            migrationBuilder.DropIndex(
                name: "IX_Carts_ColorId",
                table: "Carts");

            migrationBuilder.DropIndex(
                name: "IX_Carts_SizeId",
                table: "Carts");

            migrationBuilder.DropColumn(
                name: "ColorId",
                table: "Carts");

            migrationBuilder.DropColumn(
                name: "SizeId",
                table: "Carts");

            migrationBuilder.AddColumn<string>(
                name: "Color",
                table: "Carts",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Size",
                table: "Carts",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
