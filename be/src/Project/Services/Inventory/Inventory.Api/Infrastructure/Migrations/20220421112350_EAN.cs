using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Inventory.Api.Infrastructure.Migrations
{
    public partial class EAN : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Ean",
                table: "Inventory",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Inventory_WarehouseId",
                table: "Inventory",
                column: "WarehouseId");

            migrationBuilder.AddForeignKey(
                name: "FK_Inventory_Warehouses_WarehouseId",
                table: "Inventory",
                column: "WarehouseId",
                principalTable: "Warehouses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Inventory_Warehouses_WarehouseId",
                table: "Inventory");

            migrationBuilder.DropIndex(
                name: "IX_Inventory_WarehouseId",
                table: "Inventory");

            migrationBuilder.DropColumn(
                name: "Ean",
                table: "Inventory");
        }
    }
}
