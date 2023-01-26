using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Analytics.Api.Infrastructure.Migrations
{
    public partial class RemovedSalesFactsVirtualCollections : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ClientDimensions_SalesFacts_SalesFactId",
                table: "ClientDimensions");

            migrationBuilder.DropForeignKey(
                name: "FK_LocationDimensions_SalesFacts_SalesFactId",
                table: "LocationDimensions");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductDimensions_SalesFacts_SalesFactId",
                table: "ProductDimensions");

            migrationBuilder.DropForeignKey(
                name: "FK_TimeDimensions_SalesFacts_SalesFactId",
                table: "TimeDimensions");

            migrationBuilder.DropIndex(
                name: "IX_TimeDimensions_SalesFactId",
                table: "TimeDimensions");

            migrationBuilder.DropIndex(
                name: "IX_ProductDimensions_SalesFactId",
                table: "ProductDimensions");

            migrationBuilder.DropIndex(
                name: "IX_LocationDimensions_SalesFactId",
                table: "LocationDimensions");

            migrationBuilder.DropIndex(
                name: "IX_ClientDimensions_SalesFactId",
                table: "ClientDimensions");

            migrationBuilder.DropColumn(
                name: "SalesFactId",
                table: "TimeDimensions");

            migrationBuilder.DropColumn(
                name: "SalesFactId",
                table: "ProductDimensions");

            migrationBuilder.DropColumn(
                name: "SalesFactId",
                table: "LocationDimensions");

            migrationBuilder.DropColumn(
                name: "SalesFactId",
                table: "ClientDimensions");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "SalesFactId",
                table: "TimeDimensions",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "SalesFactId",
                table: "ProductDimensions",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "SalesFactId",
                table: "LocationDimensions",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "SalesFactId",
                table: "ClientDimensions",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_TimeDimensions_SalesFactId",
                table: "TimeDimensions",
                column: "SalesFactId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductDimensions_SalesFactId",
                table: "ProductDimensions",
                column: "SalesFactId");

            migrationBuilder.CreateIndex(
                name: "IX_LocationDimensions_SalesFactId",
                table: "LocationDimensions",
                column: "SalesFactId");

            migrationBuilder.CreateIndex(
                name: "IX_ClientDimensions_SalesFactId",
                table: "ClientDimensions",
                column: "SalesFactId");

            migrationBuilder.AddForeignKey(
                name: "FK_ClientDimensions_SalesFacts_SalesFactId",
                table: "ClientDimensions",
                column: "SalesFactId",
                principalTable: "SalesFacts",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_LocationDimensions_SalesFacts_SalesFactId",
                table: "LocationDimensions",
                column: "SalesFactId",
                principalTable: "SalesFacts",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductDimensions_SalesFacts_SalesFactId",
                table: "ProductDimensions",
                column: "SalesFactId",
                principalTable: "SalesFacts",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_TimeDimensions_SalesFacts_SalesFactId",
                table: "TimeDimensions",
                column: "SalesFactId",
                principalTable: "SalesFacts",
                principalColumn: "Id");
        }
    }
}
