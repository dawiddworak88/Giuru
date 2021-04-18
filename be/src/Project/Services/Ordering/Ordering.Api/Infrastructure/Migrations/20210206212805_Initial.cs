using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Ordering.Api.Infrastructure.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "OrderComments",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    IsActive = table.Column<bool>(nullable: false),
                    RowVersion = table.Column<byte[]>(rowVersion: true, nullable: true),
                    LastModifiedDate = table.Column<DateTime>(nullable: false),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    Text = table.Column<string>(nullable: false),
                    Language = table.Column<string>(nullable: true),
                    UserId = table.Column<Guid>(nullable: false),
                    OrderId = table.Column<Guid>(nullable: false),
                    IpAddress = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderComments", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Orders",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    IsActive = table.Column<bool>(nullable: false),
                    RowVersion = table.Column<byte[]>(rowVersion: true, nullable: true),
                    LastModifiedDate = table.Column<DateTime>(nullable: false),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    ClientId = table.Column<Guid>(nullable: true),
                    ClientName = table.Column<string>(nullable: true),
                    SellerId = table.Column<Guid>(nullable: true),
                    BillingAddressId = table.Column<Guid>(nullable: true),
                    BillingCompany = table.Column<string>(nullable: true),
                    BillingFirstName = table.Column<string>(nullable: true),
                    BillingLastName = table.Column<string>(nullable: true),
                    BillingRegion = table.Column<string>(nullable: true),
                    BillingPostCode = table.Column<string>(nullable: true),
                    BillingCity = table.Column<string>(nullable: true),
                    BillingStreet = table.Column<string>(nullable: true),
                    BillingPhonePrefix = table.Column<string>(nullable: true),
                    BillingPhone = table.Column<string>(nullable: true),
                    BillingCountryCode = table.Column<string>(nullable: true),
                    ShippingAddressId = table.Column<Guid>(nullable: true),
                    ShippingCompany = table.Column<string>(nullable: true),
                    ShippingFirstName = table.Column<string>(nullable: true),
                    ShippingLastName = table.Column<string>(nullable: true),
                    ShippingRegion = table.Column<string>(nullable: true),
                    ShippingPostCode = table.Column<string>(nullable: true),
                    ShippingCity = table.Column<string>(nullable: true),
                    ShippingStreet = table.Column<string>(nullable: true),
                    ShippingPhonePrefix = table.Column<string>(nullable: true),
                    ShippingPhone = table.Column<string>(nullable: true),
                    ShippingCountryCode = table.Column<string>(nullable: true),
                    ExternalReference = table.Column<string>(nullable: true),
                    MoreInfo = table.Column<string>(nullable: true),
                    OrderStatusId = table.Column<Guid>(nullable: false),
                    OrderStateId = table.Column<Guid>(nullable: false),
                    Reason = table.Column<string>(nullable: true),
                    IpAddress = table.Column<string>(nullable: true),
                    ExpectedDeliveryDate = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Orders", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "OrderStates",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    IsActive = table.Column<bool>(nullable: false),
                    RowVersion = table.Column<byte[]>(rowVersion: true, nullable: true),
                    LastModifiedDate = table.Column<DateTime>(nullable: false),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderStates", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "OrderStatuses",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    IsActive = table.Column<bool>(nullable: false),
                    RowVersion = table.Column<byte[]>(rowVersion: true, nullable: true),
                    LastModifiedDate = table.Column<DateTime>(nullable: false),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    OrderStateId = table.Column<Guid>(nullable: false),
                    Order = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderStatuses", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "OrderItems",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    IsActive = table.Column<bool>(nullable: false),
                    RowVersion = table.Column<byte[]>(rowVersion: true, nullable: true),
                    LastModifiedDate = table.Column<DateTime>(nullable: false),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    OrderId = table.Column<Guid>(nullable: false),
                    ProductId = table.Column<Guid>(nullable: false),
                    ProductSku = table.Column<string>(nullable: false),
                    ProductName = table.Column<string>(nullable: false),
                    Quantity = table.Column<double>(nullable: false),
                    PictureUrl = table.Column<string>(nullable: true),
                    ExternalReference = table.Column<string>(nullable: true),
                    ExpectedDeliveryFrom = table.Column<DateTime>(nullable: true),
                    ExpectedDeliveryTo = table.Column<DateTime>(nullable: true),
                    MoreInfo = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OrderItems_Orders_OrderId",
                        column: x => x.OrderId,
                        principalTable: "Orders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OrderStatusTranslations",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    IsActive = table.Column<bool>(nullable: false),
                    RowVersion = table.Column<byte[]>(rowVersion: true, nullable: true),
                    LastModifiedDate = table.Column<DateTime>(nullable: false),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    Language = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    OrderStatusId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderStatusTranslations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OrderStatusTranslations_OrderStatuses_OrderStatusId",
                        column: x => x.OrderStatusId,
                        principalTable: "OrderStatuses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_OrderItems_OrderId",
                table: "OrderItems",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderStatusTranslations_OrderStatusId",
                table: "OrderStatusTranslations",
                column: "OrderStatusId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OrderComments");

            migrationBuilder.DropTable(
                name: "OrderItems");

            migrationBuilder.DropTable(
                name: "OrderStates");

            migrationBuilder.DropTable(
                name: "OrderStatusTranslations");

            migrationBuilder.DropTable(
                name: "Orders");

            migrationBuilder.DropTable(
                name: "OrderStatuses");
        }
    }
}
