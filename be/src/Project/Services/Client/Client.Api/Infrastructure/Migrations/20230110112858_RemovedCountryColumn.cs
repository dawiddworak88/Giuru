using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Client.Api.Infrastructure.Migrations
{
    public partial class RemovedCountryColumn : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"IF EXISTS (SELECT 1
                                    FROM INFORMATION_SCHEMA.COLUMNS
                                    WHERE TABLE_NAME = 'Clients'
                                            AND COLUMN_NAME = 'Country'
                                            AND TABLE_SCHEMA='dbo')
                                    BEGIN
                                        ALTER TABLE Clients
                                        DROP COLUMN Country
                                    END
                                GO");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
