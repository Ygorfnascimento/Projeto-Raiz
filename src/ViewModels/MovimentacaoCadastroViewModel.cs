using Microsoft.AspNetCore.Mvc.Rendering;

namespace Raiz.ViewModels;

public class MovimentacaoCadastroViewModel
{
    public int MovimentacaoId { get; set; }

    public string Documento { get; set; } = string.Empty;

    public DateTime DataMovimentacao { get; set; }

    public int TipoId { get; set; }

    public int StatusId { get; set; }

    public string? TipoNome { get; set; }

    public string? StatusNome { get; set; }

    public List<SelectListItem>? TiposMovimentacoes { get; set; }

    public List<MovimentacaoItemCadastroViewModel> Itens { get; set; } = new();
}