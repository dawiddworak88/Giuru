using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ordering.Api.Infrastructure.Migrations
{
    public partial class AddedNonClusteredIndexToOrderItemStatusChangeIsActiveOrderItemId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_OrderItemStatusChanges_IsActive_OrderItemId",
                table: "OrderItemStatusChanges",
                columns: new[] { "IsActive", "OrderItemId" })
                .Annotation("SqlServer:Include", new[] { "CreatedDate", "LastModifiedDate", "OrderItemStateId", "OrderItemStatusId", "RowVersion" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_OrderItemStatusChanges_IsActive_OrderItemId",
                table: "OrderItemStatusChanges");
        }
    }
}
