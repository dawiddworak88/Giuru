using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace EventLogging.Api.Infrastructure.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "EventLogs",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    EventId = table.Column<Guid>(nullable: true),
                    EventName = table.Column<string>(nullable: true),
                    EventState = table.Column<string>(nullable: true),
                    EntityType = table.Column<string>(nullable: true),
                    EntityId = table.Column<Guid>(nullable: true),
                    OldValue = table.Column<string>(nullable: true),
                    NewValue = table.Column<string>(nullable: true),
                    Content = table.Column<string>(nullable: true),
                    OrganisationId = table.Column<Guid>(nullable: true),
                    Username = table.Column<string>(nullable: true),
                    Source = table.Column<string>(nullable: true),
                    IpAddress = table.Column<string>(nullable: true),
                    CreatedDate = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EventLogs", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EventLogs");
        }
    }
}
