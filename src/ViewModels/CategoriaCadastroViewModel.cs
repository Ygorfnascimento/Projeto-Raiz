using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace Raiz.Models;

public class CategoriaCadastroViewModel
{
    [DisplayName("id")]
    public int CategoriaId {get; set;}

    [DisplayName("Nome")]
    [Required(ErrorMessage = "O nome é obrigatório")]
    [StringLength(100, ErrorMessage = "o nome deve ter no máximo 100 caracteres.")]
    public string Nome {get; set;} = string.Empty;
}