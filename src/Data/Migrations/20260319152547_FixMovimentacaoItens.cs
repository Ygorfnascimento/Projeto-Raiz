using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Raiz.Data.Migrations
{
    /// <inheritdoc />
    public partial class FixMovimentacaoItens : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MovimentacaoItems_Movimentacoes_MovimentacaoId",
                table: "MovimentacaoItems");

            migrationBuilder.DropForeignKey(
                name: "FK_MovimentacaoItems_Produtos_ProdutoId",
                table: "MovimentacaoItems");

            migrationBuilder.DropPrimaryKey(
                name: "PK_MovimentacaoItems",
                table: "MovimentacaoItems");

            migrationBuilder.RenameTable(
                name: "MovimentacaoItems",
                newName: "MovimentacaoItens");

            migrationBuilder.RenameIndex(
                name: "IX_MovimentacaoItems_ProdutoId",
                table: "MovimentacaoItens",
                newName: "IX_MovimentacaoItens_ProdutoId");

            migrationBuilder.RenameIndex(
                name: "IX_MovimentacaoItems_MovimentacaoId",
                table: "MovimentacaoItens",
                newName: "IX_MovimentacaoItens_MovimentacaoId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_MovimentacaoItens",
                table: "MovimentacaoItens",
                column: "MovimentacaoItemId");

            migrationBuilder.AddForeignKey(
                name: "FK_MovimentacaoItens_Movimentacoes_MovimentacaoId",
                table: "MovimentacaoItens",
                column: "MovimentacaoId",
                principalTable: "Movimentacoes",
                principalColumn: "MovimentacaoId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_MovimentacaoItens_Produtos_ProdutoId",
                table: "MovimentacaoItens",
                column: "ProdutoId",
                principalTable: "Produtos",
                principalColumn: "ProdutoId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MovimentacaoItens_Movimentacoes_MovimentacaoId",
                table: "MovimentacaoItens");

            migrationBuilder.DropForeignKey(
                name: "FK_MovimentacaoItens_Produtos_ProdutoId",
                table: "MovimentacaoItens");

            migrationBuilder.DropPrimaryKey(
                name: "PK_MovimentacaoItens",
                table: "MovimentacaoItens");

            migrationBuilder.RenameTable(
                name: "MovimentacaoItens",
                newName: "MovimentacaoItems");

            migrationBuilder.RenameIndex(
                name: "IX_MovimentacaoItens_ProdutoId",
                table: "MovimentacaoItems",
                newName: "IX_MovimentacaoItems_ProdutoId");

            migrationBuilder.RenameIndex(
                name: "IX_MovimentacaoItens_MovimentacaoId",
                table: "MovimentacaoItems",
                newName: "IX_MovimentacaoItems_MovimentacaoId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_MovimentacaoItems",
                table: "MovimentacaoItems",
                column: "MovimentacaoItemId");

            migrationBuilder.AddForeignKey(
                name: "FK_MovimentacaoItems_Movimentacoes_MovimentacaoId",
                table: "MovimentacaoItems",
                column: "MovimentacaoId",
                principalTable: "Movimentacoes",
                principalColumn: "MovimentacaoId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_MovimentacaoItems_Produtos_ProdutoId",
                table: "MovimentacaoItems",
                column: "ProdutoId",
                principalTable: "Produtos",
                principalColumn: "ProdutoId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
