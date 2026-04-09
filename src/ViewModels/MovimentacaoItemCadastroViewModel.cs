using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Raiz.ViewModels;

public class MovimentacaoItemCadastroViewModel
{
    public MovimentacaoItemCadastroViewModel()
    {
        Quantidade = 1;
    }

    public int MovimentacaoItemId { get; set; }

    [DisplayName("Id da Movimentação")]
    [Required(ErrorMessage = "O Id da movimentação é obrigatório")]
    public int MovimentacaoId { get; set; }

    [DisplayName("Código de barras")]
    [Required(ErrorMessage = "O Código de barras é obrigatório")]
    public string CodigoBarras { get; set; } = string.Empty; 

    public string ProdutoNome { get; set; } = string.Empty;

    [DisplayName("Preço")]
    [Required(ErrorMessage = "O Preço é obrigatório")]
    public decimal Preco { get; set; } 

    [DisplayName("Quantidade")]
    [Required(ErrorMessage = "A Quantidade é obrigatório")]
    public int Quantidade { get; set; }
}