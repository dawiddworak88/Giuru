using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ordering.Api.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddedExtraIndexes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Orders_IsActive_CreatedDate_Id_Filtered",
                table: "Orders",
                columns: new[] { "IsActive", "CreatedDate", "Id" },
                filter: "[IsActive] = 1");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_IsActive1_CreatedDate",
                table: "Orders",
                column: "CreatedDate",
                filter: "[IsActive] = 1");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_IsActive1_CreatedDate_Id",
                table: "Orders",
                columns: new[] { "CreatedDate", "Id" },
                filter: "[IsActive] = 1")
                .Annotation("SqlServer:Include", new[] { "ClientName", "OrderStatusId", "OrderStateId" });

            migrationBuilder.CreateIndex(
                name: "IX_OrderItems_OrderId_Cover",
                table: "OrderItems",
                column: "OrderId")
                .Annotation("SqlServer:Include", new[] { "CreatedDate", "Currency", "ExternalReference", "IsActive", "LastModifiedDate", "LastOrderItemStatusChangeId", "MoreInfo", "OutletQuantity", "PictureUrl", "Price", "ProductId", "ProductName", "ProductSku", "Quantity", "RowVersion", "StockQuantity", "UnitPrice" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Orders_IsActive_CreatedDate_Id_Filtered",
                table: "Orders");

            migrationBuilder.DropIndex(
                name: "IX_Orders_IsActive1_CreatedDate",
                table: "Orders");

            migrationBuilder.DropIndex(
                name: "IX_Orders_IsActive1_CreatedDate_Id",
                table: "Orders");

            migrationBuilder.DropIndex(
                name: "IX_OrderItems_OrderId_Cover",
                table: "OrderItems");
        }
    }
}
