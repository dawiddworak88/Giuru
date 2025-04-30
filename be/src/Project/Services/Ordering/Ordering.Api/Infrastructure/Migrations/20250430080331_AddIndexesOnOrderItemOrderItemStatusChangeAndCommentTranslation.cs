using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ordering.Api.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddIndexesOnOrderItemOrderItemStatusChangeAndCommentTranslation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_OrderItemStatusChangesCommentTranslations_OrderItemStatusChangeId",
                table: "OrderItemStatusChangesCommentTranslations");

            migrationBuilder.DropIndex(
                name: "IX_OrderItemStatusChanges_IsActive_OrderItemId",
                table: "OrderItemStatusChanges");

            migrationBuilder.DropIndex(
                name: "IX_OrderItems_OrderId",
                table: "OrderItems");

            migrationBuilder.CreateIndex(
                name: "IX_OISCCT_Active_StatusChangeId",
                table: "OrderItemStatusChangesCommentTranslations",
                column: "OrderItemStatusChangeId",
                unique: true,
                filter: "[IsActive] = 1")
                .Annotation("SqlServer:Include", new[] { "OrderItemStatusChangeComment", "Language" });

            migrationBuilder.CreateIndex(
                name: "IX_OISC_Active_OrderItemId_CreatedDate_desc",
                table: "OrderItemStatusChanges",
                columns: new[] { "OrderItemId", "CreatedDate" },
                filter: "[IsActive] = 1")
                .Annotation("SqlServer:Include", new[] { "Id", "LastModifiedDate", "OrderItemStateId", "OrderItemStatusId", "RowVersion" });

            migrationBuilder.CreateIndex(
                name: "IX_OI_Active_OrderId",
                table: "OrderItems",
                column: "OrderId",
                filter: "[IsActive] = 1")
                .Annotation("SqlServer:Include", new[] { "Id", "LastOrderItemStatusChangeId", "ProductId", "ProductSku", "ProductName", "PictureUrl", "Quantity", "StockQuantity", "OutletQuantity", "ExternalReference", "MoreInfo", "LastModifiedDate", "CreatedDate" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_OISCCT_Active_StatusChangeId",
                table: "OrderItemStatusChangesCommentTranslations");

            migrationBuilder.DropIndex(
                name: "IX_OISC_Active_OrderItemId_CreatedDate_desc",
                table: "OrderItemStatusChanges");

            migrationBuilder.DropIndex(
                name: "IX_OI_Active_OrderId",
                table: "OrderItems");

            migrationBuilder.CreateIndex(
                name: "IX_OrderItemStatusChangesCommentTranslations_OrderItemStatusChangeId",
                table: "OrderItemStatusChangesCommentTranslations",
                column: "OrderItemStatusChangeId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_OrderItemStatusChanges_IsActive_OrderItemId",
                table: "OrderItemStatusChanges",
                columns: new[] { "IsActive", "OrderItemId" })
                .Annotation("SqlServer:Include", new[] { "CreatedDate", "LastModifiedDate", "OrderItemStateId", "OrderItemStatusId", "RowVersion" });

            migrationBuilder.CreateIndex(
                name: "IX_OrderItems_OrderId",
                table: "OrderItems",
                column: "OrderId");
        }
    }
}
