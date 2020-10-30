using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Media.Api.Infrastructure.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MediaItems",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    IsActive = table.Column<bool>(nullable: false),
                    LastModifiedDate = table.Column<DateTime>(nullable: false),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    OrganisationId = table.Column<Guid>(nullable: true),
                    IsProtected = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MediaItems", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MediaItemVersions",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    IsActive = table.Column<bool>(nullable: false),
                    LastModifiedDate = table.Column<DateTime>(nullable: false),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    Size = table.Column<long>(nullable: false),
                    Folder = table.Column<string>(nullable: true),
                    Filename = table.Column<string>(nullable: true),
                    Extension = table.Column<string>(nullable: true),
                    MimeType = table.Column<string>(nullable: true),
                    Version = table.Column<int>(nullable: false),
                    Checksum = table.Column<string>(nullable: true),
                    CreatedBy = table.Column<string>(nullable: true),
                    MediaItemId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MediaItemVersions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MediaItemVersions_MediaItems_MediaItemId",
                        column: x => x.MediaItemId,
                        principalTable: "MediaItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MediaItemTranslations",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    IsActive = table.Column<bool>(nullable: false),
                    LastModifiedDate = table.Column<DateTime>(nullable: false),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    Language = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    Metadata = table.Column<string>(nullable: true),
                    MediaItemVersionId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MediaItemTranslations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MediaItemTranslations_MediaItemVersions_MediaItemVersionId",
                        column: x => x.MediaItemVersionId,
                        principalTable: "MediaItemVersions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MediaItemTranslations_MediaItemVersionId",
                table: "MediaItemTranslations",
                column: "MediaItemVersionId");

            migrationBuilder.CreateIndex(
                name: "IX_MediaItemVersions_MediaItemId",
                table: "MediaItemVersions",
                column: "MediaItemId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MediaItemTranslations");

            migrationBuilder.DropTable(
                name: "MediaItemVersions");

            migrationBuilder.DropTable(
                name: "MediaItems");
        }
    }
}
