using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DigitalStore.Migrations
{
    /// <inheritdoc />
    public partial class TabelaProdutos : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Produto",
                table: "Produtos",
                newName: "NomeProduto");

            migrationBuilder.AlterColumn<string>(
                name: "ImagemUrl",
                table: "Produtos",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Favoritos_UsuarioId",
                table: "Favoritos",
                column: "UsuarioId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Carrinho_UsuarioId",
                table: "Carrinho",
                column: "UsuarioId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Carrinho_Usuarios_UsuarioId",
                table: "Carrinho",
                column: "UsuarioId",
                principalTable: "Usuarios",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Favoritos_Usuarios_UsuarioId",
                table: "Favoritos",
                column: "UsuarioId",
                principalTable: "Usuarios",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Carrinho_Usuarios_UsuarioId",
                table: "Carrinho");

            migrationBuilder.DropForeignKey(
                name: "FK_Favoritos_Usuarios_UsuarioId",
                table: "Favoritos");

            migrationBuilder.DropIndex(
                name: "IX_Favoritos_UsuarioId",
                table: "Favoritos");

            migrationBuilder.DropIndex(
                name: "IX_Carrinho_UsuarioId",
                table: "Carrinho");

            migrationBuilder.RenameColumn(
                name: "NomeProduto",
                table: "Produtos",
                newName: "Produto");

            migrationBuilder.AlterColumn<string>(
                name: "ImagemUrl",
                table: "Produtos",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");
        }
    }
}
