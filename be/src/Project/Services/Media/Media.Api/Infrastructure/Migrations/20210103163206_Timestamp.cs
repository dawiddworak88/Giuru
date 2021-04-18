using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Media.Api.Infrastructure.Migrations
{
    public partial class Timestamp : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<byte[]>(
                name: "RowVersion",
                table: "MediaItemVersions",
                rowVersion: true,
                nullable: true);

            migrationBuilder.AddColumn<byte[]>(
                name: "RowVersion",
                table: "MediaItemTranslations",
                rowVersion: true,
                nullable: true);

            migrationBuilder.AddColumn<byte[]>(
                name: "RowVersion",
                table: "MediaItems",
                rowVersion: true,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RowVersion",
                table: "MediaItemVersions");

            migrationBuilder.DropColumn(
                name: "RowVersion",
                table: "MediaItemTranslations");

            migrationBuilder.DropColumn(
                name: "RowVersion",
                table: "MediaItems");
        }
    }
}
