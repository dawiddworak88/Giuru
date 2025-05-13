using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ordering.Api.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddedIndexes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_OrderStatusTranslations_OrderStatusId",
                table: "OrderStatusTranslations");

            migrationBuilder.DropIndex(
                name: "IX_OrderItems_OrderId",
                table: "OrderItems");

            migrationBuilder.AlterColumn<string>(
                name: "Language",
                table: "OrderStatusTranslations",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ClientName",
                table: "Orders",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Language",
                table: "OrderItemStatusChangesCommentTranslations",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_OrderStatusTranslations_OrderStatusId_Language_IsActive",
                table: "OrderStatusTranslations",
                columns: new[] { "OrderStatusId", "Language", "IsActive" })
                .Annotation("SqlServer:Include", new[] { "Name" });

            migrationBuilder.CreateIndex(
                name: "IX_Orders_IsActive_ClientName",
                table: "Orders",
                columns: new[] { "IsActive", "ClientName" });

            migrationBuilder.CreateIndex(
                name: "IX_Orders_IsActive_SellerId_CreatedDate",
                table: "Orders",
                columns: new[] { "IsActive", "SellerId", "CreatedDate" })
                .Annotation("SqlServer:Include", new[] { "ClientName", "OrderStatusId", "OrderStateId" });

            migrationBuilder.CreateIndex(
                name: "IX_OrderItemStatusChangesCommentTranslations_OrderItemStatusChangeId_Language_IsActive",
                table: "OrderItemStatusChangesCommentTranslations",
                columns: new[] { "OrderItemStatusChangeId", "Language", "IsActive" })
                .Annotation("SqlServer:Include", new[] { "OrderItemStatusChangeComment" });

            migrationBuilder.CreateIndex(
                name: "IX_OrderItems_OrderId_IsActive",
                table: "OrderItems",
                columns: new[] { "OrderId", "IsActive" })
                .Annotation("SqlServer:Include", new[] { "ProductId", "ExternalReference", "LastOrderItemStatusChangeId" });

            migrationBuilder.CreateIndex(
                name: "IX_OrderAttachments_OrderId_IsActive",
                table: "OrderAttachments",
                columns: new[] { "OrderId", "IsActive" })
                .Annotation("SqlServer:Include", new[] { "MediaId" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_OrderStatusTranslations_OrderStatusId_Language_IsActive",
                table: "OrderStatusTranslations");

            migrationBuilder.DropIndex(
                name: "IX_Orders_IsActive_ClientName",
                table: "Orders");

            migrationBuilder.DropIndex(
                name: "IX_Orders_IsActive_SellerId_CreatedDate",
                table: "Orders");

            migrationBuilder.DropIndex(
                name: "IX_OrderItemStatusChangesCommentTranslations_OrderItemStatusChangeId_Language_IsActive",
                table: "OrderItemStatusChangesCommentTranslations");

            migrationBuilder.DropIndex(
                name: "IX_OrderItems_OrderId_IsActive",
                table: "OrderItems");

            migrationBuilder.DropIndex(
                name: "IX_OrderAttachments_OrderId_IsActive",
                table: "OrderAttachments");

            migrationBuilder.AlterColumn<string>(
                name: "Language",
                table: "OrderStatusTranslations",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ClientName",
                table: "Orders",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Language",
                table: "OrderItemStatusChangesCommentTranslations",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_OrderStatusTranslations_OrderStatusId",
                table: "OrderStatusTranslations",
                column: "OrderStatusId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderItems_OrderId",
                table: "OrderItems",
                column: "OrderId");
        }
    }
}
