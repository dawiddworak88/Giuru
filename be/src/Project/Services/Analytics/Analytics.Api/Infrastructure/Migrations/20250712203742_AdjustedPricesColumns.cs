using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Analytics.Api.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AdjustedPricesColumns : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Currency",
                table: "SalesFacts",
                type: "nvarchar(3)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "Price",
                table: "SalesFacts",
                type: "decimal(18,4)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Currency",
                table: "SalesFacts");

            migrationBuilder.DropColumn(
                name: "Price",
                table: "SalesFacts");
        }
    }
}
