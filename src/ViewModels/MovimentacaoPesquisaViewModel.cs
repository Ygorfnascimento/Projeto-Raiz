using System.ComponentModel;
using Microsoft.AspNetCore.Mvc.Rendering;
using Raiz.Models;

namespace Raiz.ViewModels;

public class MovimentacaoPesquisaViewModel
{
    public MovimentacaoPesquisaViewModel()
    {
        Resultado = new List<MovimentacaoCadastroViewModel>();
        TiposMovimentacoes = new List<SelectListItem>();
        Status = new List<SelectListItem>();
    }

    #region Filtros

    [DisplayName("Id")]
    public int? MovimentacaoId { get; set; }

    [DisplayName("Tipo")]
    public int? TipoId { get; set; }

    [DisplayName("Status")]
    public int? StatusId { get; set; }

    [DisplayName("Documento")]
    public string? Documento { get; set; }

    [DisplayName("Data Inicial")]
    public DateTime? DataMovimentacaoInicial { get; set; }

    [DisplayName("Data Final")]
    public DateTime? DataMovimentacaoFinal { get; set; }

    #endregion

    public List<SelectListItem> TiposMovimentacoes { get; set; }
    public List<SelectListItem> Status { get; set; }
    public IEnumerable<MovimentacaoCadastroViewModel> Resultado { get; set; }
}