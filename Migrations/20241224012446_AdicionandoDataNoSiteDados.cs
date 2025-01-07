using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DigitalStore.Migrations
{
    /// <inheritdoc />
    public partial class AdicionandoDataNoSiteDados : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Site",
                columns: new[] { "Id", "Banner1Url", "Banner2Url", "Banner3Url", "Banner4Url", "Frase", "NomeSite" },
                values: new object[] { 1, "banner-1.jpg", "", "", "", "Tudo que você procura em um só lugar", "DigitalStore" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Site",
                keyColumn: "Id",
                keyValue: 1);
        }
    }
}