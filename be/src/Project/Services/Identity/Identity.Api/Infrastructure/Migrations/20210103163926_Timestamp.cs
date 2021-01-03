using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Identity.Api.Infrastructure.Migrations
{
    public partial class Timestamp : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<byte[]>(
                name: "RowVersion",
                table: "OrganisationVideos",
                rowVersion: true,
                nullable: true);

            migrationBuilder.AddColumn<byte[]>(
                name: "RowVersion",
                table: "OrganisationTranslations",
                rowVersion: true,
                nullable: true);

            migrationBuilder.AddColumn<byte[]>(
                name: "RowVersion",
                table: "Organisations",
                rowVersion: true,
                nullable: true);

            migrationBuilder.AddColumn<byte[]>(
                name: "RowVersion",
                table: "OrganisationImages",
                rowVersion: true,
                nullable: true);

            migrationBuilder.AddColumn<byte[]>(
                name: "RowVersion",
                table: "OrganisationFiles",
                rowVersion: true,
                nullable: true);

            migrationBuilder.AddColumn<byte[]>(
                name: "RowVersion",
                table: "OrganisationAppSecrets",
                rowVersion: true,
                nullable: true);

            migrationBuilder.AddColumn<byte[]>(
                name: "RowVersion",
                table: "OrganisationAddreses",
                rowVersion: true,
                nullable: true);

            migrationBuilder.AddColumn<byte[]>(
                name: "RowVersion",
                table: "Connections",
                rowVersion: true,
                nullable: true);

            migrationBuilder.AddColumn<byte[]>(
                name: "RowVersion",
                table: "Clients",
                rowVersion: true,
                nullable: true);

            migrationBuilder.AddColumn<byte[]>(
                name: "RowVersion",
                table: "Addresses",
                rowVersion: true,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RowVersion",
                table: "OrganisationVideos");

            migrationBuilder.DropColumn(
                name: "RowVersion",
                table: "OrganisationTranslations");

            migrationBuilder.DropColumn(
                name: "RowVersion",
                table: "Organisations");

            migrationBuilder.DropColumn(
                name: "RowVersion",
                table: "OrganisationImages");

            migrationBuilder.DropColumn(
                name: "RowVersion",
                table: "OrganisationFiles");

            migrationBuilder.DropColumn(
                name: "RowVersion",
                table: "OrganisationAppSecrets");

            migrationBuilder.DropColumn(
                name: "RowVersion",
                table: "OrganisationAddreses");

            migrationBuilder.DropColumn(
                name: "RowVersion",
                table: "Connections");

            migrationBuilder.DropColumn(
                name: "RowVersion",
                table: "Clients");

            migrationBuilder.DropColumn(
                name: "RowVersion",
                table: "Addresses");
        }
    }
}
