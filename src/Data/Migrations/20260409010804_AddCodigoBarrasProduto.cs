using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Raiz.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddCodigoBarrasProduto : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CodigoBarras",
                table: "Produtos",
                type: "TEXT",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<decimal>(
                name: "Preco",
                table: "MovimentacaoItens",
                type: "decimal(10,2)",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "REAL");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CodigoBarras",
                table: "Produtos");

            migrationBuilder.AlterColumn<double>(
                name: "Preco",
                table: "MovimentacaoItens",
                type: "REAL",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(10,2)");
        }
    }
}
