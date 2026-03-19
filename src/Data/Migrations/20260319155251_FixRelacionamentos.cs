using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Raiz.Data.Migrations
{
    /// <inheritdoc />
    public partial class FixRelacionamentos : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MovimentacaoItens_Produtos_ProdutoId",
                table: "MovimentacaoItens");

            migrationBuilder.AddForeignKey(
                name: "FK_MovimentacaoItens_Produtos_ProdutoId",
                table: "MovimentacaoItens",
                column: "ProdutoId",
                principalTable: "Produtos",
                principalColumn: "ProdutoId",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MovimentacaoItens_Produtos_ProdutoId",
                table: "MovimentacaoItens");

            migrationBuilder.AddForeignKey(
                name: "FK_MovimentacaoItens_Produtos_ProdutoId",
                table: "MovimentacaoItens",
                column: "ProdutoId",
                principalTable: "Produtos",
                principalColumn: "ProdutoId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
