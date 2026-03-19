using System.ComponentModel;
using Microsoft.AspNetCore.Mvc.Rendering;
using Raiz.Models;

namespace Raiz.ViewModels;

public class ProdutoSearch
{
    #region Filtros

    [DisplayName("id")]
    public int? ProdutoId { get; set; }

    [DisplayName("Nome")]
    public string? Nome { get; set; }

    [DisplayName("Preço Inicial")]
    public double? PrecoInicial { get; set; }

    [DisplayName("Preço Final")]
    public double? PrecoFinal { get; set; }

    [DisplayName("Categoria")]
    public int? CategoriaId { get; set; }

    #endregion
    public IEnumerable<SelectListItem> CategoriasSelect { get; set; } = new List<SelectListItem>();

    public List<Produto> Resultado { get; set; } = new();
}   