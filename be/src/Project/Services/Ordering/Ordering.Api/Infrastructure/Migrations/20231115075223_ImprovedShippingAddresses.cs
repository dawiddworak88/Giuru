using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ordering.Api.Infrastructure.Migrations
{
    public partial class ImprovedShippingAddresses : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ShippingCountryCode",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "ShippingPhone",
                table: "Orders");

            migrationBuilder.RenameColumn(
                name: "ShippingPhonePrefix",
                table: "Orders",
                newName: "ShippingPhoneNumber");

            migrationBuilder.AddColumn<Guid>(
                name: "ShippingCountryId",
                table: "Orders",
                type: "uniqueidentifier",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ShippingCountryId",
                table: "Orders");

            migrationBuilder.RenameColumn(
                name: "ShippingPhoneNumber",
                table: "Orders",
                newName: "ShippingPhonePrefix");

            migrationBuilder.AddColumn<string>(
                name: "ShippingCountryCode",
                table: "Orders",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ShippingPhone",
                table: "Orders",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
