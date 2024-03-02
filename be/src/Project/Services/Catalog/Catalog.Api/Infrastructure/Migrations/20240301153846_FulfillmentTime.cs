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
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FulfillmentTime",
                table: "Products");
        }
    }
}
