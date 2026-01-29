using System.ComponentModel.DataAnnotations;

namespace Raiz.Models;

public class Categoria
{
    public int CategoriaId { get; set; }

    [Required]
    [StringLength(100)]
    public string Nome { get; set; } = string.Empty;
}
