using Foundation.Catalog.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Catalog.Api.Infrastructure.Migrations
{
    [DbContext(typeof(CatalogContext))]
    [Migration("20260610120000_AddProductIndexingIndexes")]
    public partial class AddProductIndexingIndexes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_ProductImages_ProductId",
                table: "ProductImages",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductVideos_ProductId",
                table: "ProductVideos",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductFiles_ProductId",
                table: "ProductFiles",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_Brands_SellerId",
                table: "Brands",
                column: "SellerId");

            migrationBuilder.CreateIndex(
                name: "IX_Products_PrimaryProductId",
                table: "Products",
                column: "PrimaryProductId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_ProductImages_ProductId",
                table: "ProductImages");

            migrationBuilder.DropIndex(
                name: "IX_ProductVideos_ProductId",
                table: "ProductVideos");

            migrationBuilder.DropIndex(
                name: "IX_ProductFiles_ProductId",
                table: "ProductFiles");

            migrationBuilder.DropIndex(
                name: "IX_Brands_SellerId",
                table: "Brands");

            migrationBuilder.DropIndex(
                name: "IX_Products_PrimaryProductId",
                table: "Products");
        }
    }
}
