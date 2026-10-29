using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ordering.Api.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddedExternalReferencePrefix : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ExternalReferencePrefix",
                table: "OrderItems",
                type: "nvarchar(450)",
                maxLength: 450,
                nullable: true,
                computedColumnSql: "CONVERT(nvarchar(450), LEFT([ExternalReference], 450))",
                stored: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ExternalReferencePrefix",
                table: "OrderItems");
        }
    }
}
