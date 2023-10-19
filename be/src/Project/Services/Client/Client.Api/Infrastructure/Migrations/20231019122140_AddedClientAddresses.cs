using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Client.Api.Infrastructure.Migrations
{
    public partial class AddedClientAddresses : Migration
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

            migrationBuilder.DropColumn(
                name: "Phone",
                table: "Addresses");

            migrationBuilder.RenameColumn(
                name: "PhonePrefix",
                table: "Addresses",
                newName: "PhoneNumber");

            migrationBuilder.AddColumn<Guid>(
                name: "DefaultAddressId",
                table: "Clients",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "CountryId",
                table: "Addresses",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DefaultAddressId",
                table: "Clients");

            migrationBuilder.DropColumn(
                name: "CountryId",
                table: "Addresses");

            migrationBuilder.RenameColumn(
                name: "PhoneNumber",
                table: "Addresses",
                newName: "PhonePrefix");

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

            migrationBuilder.AddColumn<string>(
                name: "Phone",
                table: "Addresses",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
