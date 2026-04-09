using System.ComponentModel;

namespace Raiz.Models;

public class CategoriaPesquisaViewModel
{
    public CategoriaPesquisaViewModel()
    {
        Resultados = new List<CategoriaCadastroViewModel>();
    }

    [DisplayName("Nome")]
    public string Nome { get; set; } = string.Empty;

    public List<CategoriaCadastroViewModel> Resultados { get; set; }
}