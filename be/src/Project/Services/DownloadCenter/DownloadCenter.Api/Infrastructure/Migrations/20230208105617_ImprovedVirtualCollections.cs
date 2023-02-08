using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DownloadCenter.Api.Infrastructure.Migrations
{
    public partial class ImprovedVirtualCollections : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DownloadCenterCategoryTranslations_DownloadCenterCategories_DownloadCenterCategoryId",
                table: "DownloadCenterCategoryTranslations");

            migrationBuilder.DropIndex(
                name: "IX_DownloadCenterCategoryTranslations_DownloadCenterCategoryId",
                table: "DownloadCenterCategoryTranslations");

            migrationBuilder.DropColumn(
                name: "DownloadCenterCategoryId",
                table: "DownloadCenterCategoryTranslations");

            migrationBuilder.CreateIndex(
                name: "IX_DownloadCenterCategoryTranslations_CategoryId",
                table: "DownloadCenterCategoryTranslations",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_DownloadCenterCategoryFiles_CategoryId",
                table: "DownloadCenterCategoryFiles",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_DownloadCenterCategories_ParentCategoryId",
                table: "DownloadCenterCategories",
                column: "ParentCategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_DownloadCenterCategories_DownloadCenterCategories_ParentCategoryId",
                table: "DownloadCenterCategories",
                column: "ParentCategoryId",
                principalTable: "DownloadCenterCategories",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_DownloadCenterCategoryFiles_DownloadCenterCategories_CategoryId",
                table: "DownloadCenterCategoryFiles",
                column: "CategoryId",
                principalTable: "DownloadCenterCategories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_DownloadCenterCategoryTranslations_DownloadCenterCategories_CategoryId",
                table: "DownloadCenterCategoryTranslations",
                column: "CategoryId",
                principalTable: "DownloadCenterCategories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DownloadCenterCategories_DownloadCenterCategories_ParentCategoryId",
                table: "DownloadCenterCategories");

            migrationBuilder.DropForeignKey(
                name: "FK_DownloadCenterCategoryFiles_DownloadCenterCategories_CategoryId",
                table: "DownloadCenterCategoryFiles");

            migrationBuilder.DropForeignKey(
                name: "FK_DownloadCenterCategoryTranslations_DownloadCenterCategories_CategoryId",
                table: "DownloadCenterCategoryTranslations");

            migrationBuilder.DropIndex(
                name: "IX_DownloadCenterCategoryTranslations_CategoryId",
                table: "DownloadCenterCategoryTranslations");

            migrationBuilder.DropIndex(
                name: "IX_DownloadCenterCategoryFiles_CategoryId",
                table: "DownloadCenterCategoryFiles");

            migrationBuilder.DropIndex(
                name: "IX_DownloadCenterCategories_ParentCategoryId",
                table: "DownloadCenterCategories");

            migrationBuilder.AddColumn<Guid>(
                name: "DownloadCenterCategoryId",
                table: "DownloadCenterCategoryTranslations",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_DownloadCenterCategoryTranslations_DownloadCenterCategoryId",
                table: "DownloadCenterCategoryTranslations",
                column: "DownloadCenterCategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_DownloadCenterCategoryTranslations_DownloadCenterCategories_DownloadCenterCategoryId",
                table: "DownloadCenterCategoryTranslations",
                column: "DownloadCenterCategoryId",
                principalTable: "DownloadCenterCategories",
                principalColumn: "Id");
        }
    }
}
