using Microsoft.AspNetCore.Mvc.Rendering;
using Raiz.Models;

namespace Raiz.ViewModels
{
    public class ProdutoRegister
    {
        public Produto Produto { get; set; } = new Produto();

        public List<SelectListItem> Categorias { get; set; } = new();
    }
}
