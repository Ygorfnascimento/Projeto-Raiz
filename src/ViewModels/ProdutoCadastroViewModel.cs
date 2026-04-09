using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Raiz.ViewModels
{
    public class ProdutoCadastroViewModel
    {
        public ProdutoCadastroViewModel()
        {
            Categorias = new List<SelectListItem>();
        }

        public int ProdutoId { get; set; }

        [DisplayName("Código de Barras")]
        [Required(ErrorMessage = "O código de barras é obrigatório")]
        public string CodigoBarras { get; set; } = string.Empty;

        [DisplayName("Nome")]
        [Required(ErrorMessage = "O nome do produto é obrigatório")]
        public string Nome { get; set; } = string.Empty;

        [DisplayName("Descrição")]
        public string Descricao { get; set; } = string.Empty;

        [DisplayName("Categoria")]
        [Required(ErrorMessage = "Selecione uma categoria")]
        public int CategoriaId { get; set; }

        public string? CategoriaNome { get; set; }

        [DisplayName("Preço")]
        [Required(ErrorMessage = "O preço é obrigatório")]
        public decimal Preco { get; set; }

        [DisplayName("Quantidade")]
        public int QuantidadeEstoque { get; set; }

        public List<SelectListItem> Categorias { get; set; }
    }
}