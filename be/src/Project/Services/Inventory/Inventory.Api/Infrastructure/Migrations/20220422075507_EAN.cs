using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Inventory.Api.Infrastructure.Migrations
{
    public partial class EAN : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Inventory_Warehouses_WarehouseId",
                table: "Inventory");

            migrationBuilder.DropForeignKey(
                name: "FK_Outlet_Warehouses_WarehouseId",
                table: "Outlet");

            migrationBuilder.DropIndex(
                name: "IX_Outlet_WarehouseId",
                table: "Outlet");

            migrationBuilder.DropIndex(
                name: "IX_Inventory_WarehouseId",
                table: "Inventory");

            migrationBuilder.AddColumn<string>(
                name: "Ean",
                table: "Outlet",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Ean",
                table: "Inventory",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "OutletTranslations",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Language = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    OutletItemId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true),
                    LastModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OutletTranslations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OutletTranslations_Outlet_OutletItemId",
                        column: x => x.OutletItemId,
                        principalTable: "Outlet",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_OutletTranslations_OutletItemId",
                table: "OutletTranslations",
                column: "OutletItemId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OutletTranslations");

            migrationBuilder.DropColumn(
                name: "Ean",
                table: "Outlet");

            migrationBuilder.DropColumn(
                name: "Ean",
                table: "Inventory");

            migrationBuilder.CreateIndex(
                name: "IX_Outlet_WarehouseId",
                table: "Outlet",
                column: "WarehouseId");

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

            migrationBuilder.AddForeignKey(
                name: "FK_Outlet_Warehouses_WarehouseId",
                table: "Outlet",
                column: "WarehouseId",
                principalTable: "Warehouses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
