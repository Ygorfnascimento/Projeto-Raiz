using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Raiz.Models;

public class MovimentacaoItem
{
    public int MovimentacaoItemId { get; set; }

    [Required]
    public int MovimentacaoId { get; set; }

    [Required]
    public int ProdutoId { get; set; }

    [Required]
    [Range(0.01, double.MaxValue, ErrorMessage = "Preço deve ser maior que zero")]
    [Column(TypeName = "decimal(10,2)")]
    public decimal Preco { get; set; }

    [Required]
    [Range(1, int.MaxValue, ErrorMessage = "Quantidade deve ser maior que zero")]
    public int Quantidade { get; set; }

    public Movimentacao? Movimentacao { get; set; }

    public Produto? Produto { get; set; }
}