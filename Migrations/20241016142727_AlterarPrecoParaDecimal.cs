using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DigitalStore.Migrations
{
    /// <inheritdoc />
    public partial class AlterarPrecoParaDecimal : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Endereco",
                table: "Usuarios",
                newName: "Endereço");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Endereço",
                table: "Usuarios",
                newName: "Endereco");
        }
    }
}
