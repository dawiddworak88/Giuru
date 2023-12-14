using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Global.Api.Infrastructure.Migrations
{
    public partial class AddedTranslationToCurrency : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_CurrenciesTranslations_CurrencyId",
                table: "CurrenciesTranslations",
                column: "CurrencyId");

            migrationBuilder.AddForeignKey(
                name: "FK_CurrenciesTranslations_Currencies_CurrencyId",
                table: "CurrenciesTranslations",
                column: "CurrencyId",
                principalTable: "Currencies",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CurrenciesTranslations_Currencies_CurrencyId",
                table: "CurrenciesTranslations");

            migrationBuilder.DropIndex(
                name: "IX_CurrenciesTranslations_CurrencyId",
                table: "CurrenciesTranslations");
        }
    }
}
