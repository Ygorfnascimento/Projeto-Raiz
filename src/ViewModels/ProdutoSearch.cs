using System.ComponentModel;
using Microsoft.AspNetCore.Mvc.Rendering;
using Raiz.Models;

namespace Raiz.ViewModels;

public class ProdutoSearch
{
    #region Filtros

    [DisplayName("id")]
    public int? ProdutoId {get; set;}

    [DisplayName("Nome")]
    public string? Nome {get; set;}

    [DisplayName("Preço Inicial")]
    public double? PrecoInicial {get; set;}

    [DisplayName("Preço FInal")]
    public double? PrecoFinal {get; set;}

    [DisplayName("Categoria")]
    public int? CategoriaId {get; set;}

    #endregion

    public List<SelectListItem> Categorias {get; set;}

    public IEnumerable<Produto> Resultado {get; set;}
}