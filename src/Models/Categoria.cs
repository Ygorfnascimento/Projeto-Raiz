using System.ComponentModel.DataAnnotations;

namespace Raiz.Models;

public class Categoria
{
    public virtual ICollection<Produto> Produtos { get; set; } = new List<Produto>();
        public int CategoriaId { get; set; }

    [Required]
    [StringLength(100)]
    public string Nome { get; set; } = string.Empty;
}
