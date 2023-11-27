using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ordering.Api.Infrastructure.Migrations
{
    public partial class ImprovedBillingAddresses : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BillingCountryCode",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "BillingPhone",
                table: "Orders");

            migrationBuilder.RenameColumn(
                name: "BillingPhonePrefix",
                table: "Orders",
                newName: "BillingPhoneNumber");

            migrationBuilder.AddColumn<Guid>(
                name: "BillingCountryId",
                table: "Orders",
                type: "uniqueidentifier",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BillingCountryId",
                table: "Orders");

            migrationBuilder.RenameColumn(
                name: "BillingPhoneNumber",
                table: "Orders",
                newName: "BillingPhonePrefix");

            migrationBuilder.AddColumn<string>(
                name: "BillingCountryCode",
                table: "Orders",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "BillingPhone",
                table: "Orders",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
