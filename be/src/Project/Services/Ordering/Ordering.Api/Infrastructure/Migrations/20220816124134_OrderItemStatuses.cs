using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ordering.Api.Infrastructure.Migrations
{
    public partial class OrderItemStatuses : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "LastOrderItemStatusChangeId",
                table: "OrderItems",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "OrderItemStatusChanges",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    OrderItemId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    OrderItemStatusId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    OrderItemStateId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true),
                    LastModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderItemStatusChanges", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "OrderItemStatusChangesCommentTranslations",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    OrderItemStatusChangeComment = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    OrderItemStatusChangeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true),
                    LastModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Language = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderItemStatusChangesCommentTranslations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OrderItemStatusChangesCommentTranslations_OrderItemStatusChanges_OrderItemStatusChangeId",
                        column: x => x.OrderItemStatusChangeId,
                        principalTable: "OrderItemStatusChanges",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_OrderItemStatusChangesCommentTranslations_OrderItemStatusChangeId",
                table: "OrderItemStatusChangesCommentTranslations",
                column: "OrderItemStatusChangeId",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OrderItemStatusChangesCommentTranslations");

            migrationBuilder.DropTable(
                name: "OrderItemStatusChanges");

            migrationBuilder.DropColumn(
                name: "LastOrderItemStatusChangeId",
                table: "OrderItems");
        }
    }
}
