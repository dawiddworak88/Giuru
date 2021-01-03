using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Catalog.Api.Infrastructure.Migrations
{
    public partial class Timestamp : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<byte[]>(
                name: "RowVersion",
                table: "TaxonomyTranslations",
                rowVersion: true,
                nullable: true);

            migrationBuilder.AddColumn<byte[]>(
                name: "RowVersion",
                table: "TaxonomyImages",
                rowVersion: true,
                nullable: true);

            migrationBuilder.AddColumn<byte[]>(
                name: "RowVersion",
                table: "Taxonomies",
                rowVersion: true,
                nullable: true);

            migrationBuilder.AddColumn<byte[]>(
                name: "RowVersion",
                table: "SchemaTranslations",
                rowVersion: true,
                nullable: true);

            migrationBuilder.AddColumn<byte[]>(
                name: "RowVersion",
                table: "Schemas",
                rowVersion: true,
                nullable: true);

            migrationBuilder.AddColumn<byte[]>(
                name: "RowVersion",
                table: "ProductVideos",
                rowVersion: true,
                nullable: true);

            migrationBuilder.AddColumn<byte[]>(
                name: "RowVersion",
                table: "ProductTranslations",
                rowVersion: true,
                nullable: true);

            migrationBuilder.AddColumn<byte[]>(
                name: "RowVersion",
                table: "Products",
                rowVersion: true,
                nullable: true);

            migrationBuilder.AddColumn<byte[]>(
                name: "RowVersion",
                table: "ProductImages",
                rowVersion: true,
                nullable: true);

            migrationBuilder.AddColumn<byte[]>(
                name: "RowVersion",
                table: "ProductFiles",
                rowVersion: true,
                nullable: true);

            migrationBuilder.AddColumn<byte[]>(
                name: "RowVersion",
                table: "CategoryTranslations",
                rowVersion: true,
                nullable: true);

            migrationBuilder.AddColumn<byte[]>(
                name: "RowVersion",
                table: "CategoryImages",
                rowVersion: true,
                nullable: true);

            migrationBuilder.AddColumn<byte[]>(
                name: "RowVersion",
                table: "Categories",
                rowVersion: true,
                nullable: true);

            migrationBuilder.AddColumn<byte[]>(
                name: "RowVersion",
                table: "Brands",
                rowVersion: true,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RowVersion",
                table: "TaxonomyTranslations");

            migrationBuilder.DropColumn(
                name: "RowVersion",
                table: "TaxonomyImages");

            migrationBuilder.DropColumn(
                name: "RowVersion",
                table: "Taxonomies");

            migrationBuilder.DropColumn(
                name: "RowVersion",
                table: "SchemaTranslations");

            migrationBuilder.DropColumn(
                name: "RowVersion",
                table: "Schemas");

            migrationBuilder.DropColumn(
                name: "RowVersion",
                table: "ProductVideos");

            migrationBuilder.DropColumn(
                name: "RowVersion",
                table: "ProductTranslations");

            migrationBuilder.DropColumn(
                name: "RowVersion",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "RowVersion",
                table: "ProductImages");

            migrationBuilder.DropColumn(
                name: "RowVersion",
                table: "ProductFiles");

            migrationBuilder.DropColumn(
                name: "RowVersion",
                table: "CategoryTranslations");

            migrationBuilder.DropColumn(
                name: "RowVersion",
                table: "CategoryImages");

            migrationBuilder.DropColumn(
                name: "RowVersion",
                table: "Categories");

            migrationBuilder.DropColumn(
                name: "RowVersion",
                table: "Brands");
        }
    }
}
