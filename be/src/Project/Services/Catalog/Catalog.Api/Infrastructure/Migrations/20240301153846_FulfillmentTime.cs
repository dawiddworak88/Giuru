using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Catalog.Api.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class FulfillmentTime : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "FulfillmentTime",
                table: "Products",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_CategoryImages_CategoryId",
                table: "CategoryImages",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Categories_Parentid",
                table: "Categories",
                column: "Parentid");

            migrationBuilder.AddForeignKey(
                name: "FK_Categories_Categories_Parentid",
                table: "Categories",
                column: "Parentid",
                principalTable: "Categories",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_CategoryImages_Categories_CategoryId",
                table: "CategoryImages",
                column: "CategoryId",
                principalTable: "Categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Categories_Categories_Parentid",
                table: "Categories");

            migrationBuilder.DropForeignKey(
                name: "FK_CategoryImages_Categories_CategoryId",
                table: "CategoryImages");

            migrationBuilder.DropIndex(
                name: "IX_CategoryImages_CategoryId",
                table: "CategoryImages");

            migrationBuilder.DropIndex(
                name: "IX_Categories_Parentid",
                table: "Categories");

            migrationBuilder.DropColumn(
                name: "FulfillmentTime",
                table: "Products");
        }
    }
}
