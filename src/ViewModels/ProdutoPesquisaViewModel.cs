using System.ComponentModel;
using Microsoft.AspNetCore.Mvc.Rendering;
using Raiz.Models;

namespace Raiz.ViewModels;

public class ProdutoPesquisaViewModel
{
    public ProdutoPesquisaViewModel()
    {
        Categorias = new List<SelectListItem>();
        Resultados = new List<ProdutoCadastroViewModel>();
    }

    [DisplayName("Id")]
    public int? ProdutoId { get; set; }

    [DisplayName("Código de Barras")]
    public string? CodigoBarras { get; set; }

    [DisplayName("Nome do Produto")]
    public string? Nome { get; set; }

    [DisplayName("Preço Mínimo")]
    public decimal? PrecoInicial { get; set; }

    [DisplayName("Preço Máximo")]
    public decimal? PrecoFinal { get; set; }

    [DisplayName("Categoria")]
    public int? CategoriaId { get; set; }

    [DisplayName("Estoque Mínimo")]
    public int? QuantidadeInicial { get; set; }

    [DisplayName("Estoque Máximo")]
    public int? QuantidadeFinal { get; set; }

    public List<SelectListItem> Categorias { get; set; }

    public List<ProdutoCadastroViewModel> Resultados { get; set; }
}