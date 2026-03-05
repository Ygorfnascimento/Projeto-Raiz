using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Raiz.Models;

public class Produto
{
    [DisplayName("Id")]
    public int ProdutoId {get; set;}

    [Required]
    [StringLength(100)]

    public string Nome {get; set;} = string.Empty;

    [Required]
    [StringLength(100)]
    [DisplayName("Descrição")]

    public string Descricao {get; set;} = string.Empty;
    [DisplayName("Categoria")]
    public int CategoriaId {get; set;}
    [DisplayName("Preço")]
    public double Preco {get; set;}

    public Categoria? Categoria {get; set;}
}