using System.ComponentModel.DataAnnotations;

namespace Raiz.Models;

public class Produto
{
    public int ProdutoId {get; set;}

    [Required]
    [StringLength(100)]

    public string Nome {get; set;} = string.Empty;

    [Required]
    [StringLength(100)]

    public string Descricao {get; set;} = string.Empty;

    public int CategoriaId {get; set;}

    public double Preco {get; set;}

    public Categoria? Categoria {get; set;}
}