using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Client.Api.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddedAddressesToApplicationAndRemovedCompanyFields : Migration
    {

        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CompanyAddress",
                table: "ClientsApplications");

            migrationBuilder.DropColumn(
                name: "CompanyCity",
                table: "ClientsApplications");

            migrationBuilder.DropColumn(
                name: "CompanyCountry",
                table: "ClientsApplications");

            migrationBuilder.DropColumn(
                name: "CompanyPostalCode",
                table: "ClientsApplications");

            migrationBuilder.RenameColumn(
                name: "CompanyRegion",
                table: "ClientsApplications",
                newName: "CommunicationLanguage");

            migrationBuilder.AddColumn<Guid>(
                name: "BillingAddressId",
                table: "ClientsApplications",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "DeliveryAddressId",
                table: "ClientsApplications",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<bool>(
                name: "IsDeliveryAddressEqualBillingAddress",
                table: "ClientsApplications",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "ClientsApplicationAddresses",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FullName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Street = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Region = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PostalCode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    City = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Country = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true),
                    LastModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClientsApplicationAddresses", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CompanyAddress",
                table: "ClientsApplications",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "CompanyCity",
                table: "ClientsApplications",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "CompanyCountry",
                table: "ClientsApplications",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "CompanyPostalCode",
                table: "ClientsApplications",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.DropTable(
               name: "ClientsApplicationAddresses");

            migrationBuilder.DropColumn(
                name: "BillingAddressId",
                table: "ClientsApplications");

            migrationBuilder.DropColumn(
                name: "DeliveryAddressId",
                table: "ClientsApplications");

            migrationBuilder.DropColumn(
                name: "IsDeliveryAddressEqualBillingAddress",
                table: "ClientsApplications");

            migrationBuilder.RenameColumn(
                name: "CommunicationLanguage",
                table: "ClientsApplications",
                newName: "CompanyRegion");
        }
    }
}
