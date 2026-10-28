using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ordering.Api.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddedExternalReferencePrefixIndex : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_OrderItems_ExternalReferencePrefix",
                table: "OrderItems",
                column: "ExternalReferencePrefix")
                .Annotation("SqlServer:Include", new[] { "OrderId", "IsActive" })
                .Annotation("SqlServer:Online", true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_OrderItems_ExternalReferencePrefix",
                table: "OrderItems");
        }
    }
}
