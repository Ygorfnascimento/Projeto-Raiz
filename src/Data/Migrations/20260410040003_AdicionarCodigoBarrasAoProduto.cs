using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Raiz.Data.Migrations
{
    /// <inheritdoc />
    public partial class AdicionarCodigoBarrasAoProduto : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "QuantidadeEstoque",
                table: "Produtos",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "REAL");

            migrationBuilder.AlterColumn<decimal>(
                name: "Preco",
                table: "Produtos",
                type: "TEXT",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "REAL");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<double>(
                name: "QuantidadeEstoque",
                table: "Produtos",
                type: "REAL",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.AlterColumn<double>(
                name: "Preco",
                table: "Produtos",
                type: "REAL",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "TEXT");
        }
    }
}
