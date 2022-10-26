using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Client.Api.Infrastructure.Migrations
{
    public partial class ClientCountry : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Country",
                table: "Clients",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Country",
                table: "Clients");
        }
    }
}
