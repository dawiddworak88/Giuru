using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Client.Api.Infrastructure.Migrations
{
    public partial class AddedClientDeliveryAddresses : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Company",
                table: "Addresses");

            migrationBuilder.DropColumn(
                name: "CountryCode",
                table: "Addresses");

            migrationBuilder.DropColumn(
                name: "FirstName",
                table: "Addresses");

            migrationBuilder.DropColumn(
                name: "LastName",
                table: "Addresses");

            migrationBuilder.RenameColumn(
                name: "PhonePrefix",
                table: "Addresses",
                newName: "Recipient");

            migrationBuilder.RenameColumn(
                name: "Phone",
                table: "Addresses",
                newName: "PhoneNumber");

            migrationBuilder.AddColumn<Guid>(
                name: "DefaultDeliveryAddressId",
                table: "Clients",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "CountryId",
                table: "Addresses",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Addresses_ClientId",
                table: "Addresses",
                column: "ClientId");

            migrationBuilder.AddForeignKey(
                name: "FK_Addresses_Clients_ClientId",
                table: "Addresses",
                column: "ClientId",
                principalTable: "Clients",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Addresses_Clients_ClientId",
                table: "Addresses");

            migrationBuilder.DropIndex(
                name: "IX_Addresses_ClientId",
                table: "Addresses");

            migrationBuilder.DropColumn(
                name: "DefaultDeliveryAddressId",
                table: "Clients");

            migrationBuilder.DropColumn(
                name: "CountryId",
                table: "Addresses");

            migrationBuilder.RenameColumn(
                name: "Recipient",
                table: "Addresses",
                newName: "PhonePrefix");

            migrationBuilder.RenameColumn(
                name: "PhoneNumber",
                table: "Addresses",
                newName: "Phone");

            migrationBuilder.AddColumn<string>(
                name: "Company",
                table: "Addresses",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CountryCode",
                table: "Addresses",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "FirstName",
                table: "Addresses",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LastName",
                table: "Addresses",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
