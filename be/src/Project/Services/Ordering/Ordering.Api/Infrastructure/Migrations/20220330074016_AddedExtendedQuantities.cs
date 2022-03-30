using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ordering.Api.Infrastructure.Migrations
{
    public partial class AddedExtendedQuantities : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "OutletQuantity",
                table: "OrderItems",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "StockQuantity",
                table: "OrderItems",
                type: "float",
                nullable: false,
                defaultValue: 0.0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OutletQuantity",
                table: "OrderItems");

            migrationBuilder.DropColumn(
                name: "StockQuantity",
                table: "OrderItems");
        }
    }
}
