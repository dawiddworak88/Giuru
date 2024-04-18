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
            migrationBuilder.CreateTable(
                name: "ClientsApplicationAddresses",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FullName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Street = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Region = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PostalCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    City = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Country = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true),
                    LastModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClientsApplicationAddresses", x => x.Id);
                });

            migrationBuilder.Sql(
                "INSERT INTO ClientsApplicationAddress (FullName, PhoneNumber, Street, Region, PostalCode, City, Country) " +
                "SELECT CONCAT(FirstName, ' ', LastName) AS FullName, PhoneNumber, CompanyAddress, CompanyRegion, CompanyPostalCode, CompanyCity, CompanyCountry " +
                "FROM ClientsApplication");

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

            migrationBuilder.Sql(
                "UPDATE ClientsApplication " +
                "SET FirstName = (SELECT SUBSTRING(FullName, 1, CHARINDEX(' ', FullName) - 1)), " +
                "    LastName = (SELECT SUBSTRING(FullName, CHARINDEX(' ', FullName) + 1, LEN(FullName) - CHARINDEX(' ', FullName))), " +
                "    PhoneNumber = (SELECT PhoneNumber), " +
                "    CompanyAddress = (SELECT Street), " +
                "    CompanyRegion = (SELECT Region), " +
                "    CompanyPostalCode = (SELECT PostalCode), " +
                "    CompanyCity = (SELECT City), " +
                "    CompanyCountry = (SELECT Country) " +
                "FROM ClientsApplicationAddress " +
                "WHERE ClientsApplication.BillingAddressId = ClientsApplicationAddress.Id");

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
