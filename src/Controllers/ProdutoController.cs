using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Raiz.Data;
using Raiz.Models;
using Raiz.ViewModels;

namespace Raiz.Controllers
{
    public class ProdutoController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ProdutoController(ApplicationDbContext context)
        {
            _context = context;
        }

        // LISTAGEM COM FILTROS
        [HttpGet]
        public IActionResult Index([FromQuery] ProdutoPesquisaViewModel model)
        {
            var query = _context.Produtos.Include(p => p.Categoria).AsNoTracking();

            // Filtros
            if (!string.IsNullOrEmpty(model.CodigoBarras))
            {
                query = query.Where(x => x.CodigoBarras == model.CodigoBarras);
            }

            if (!string.IsNullOrEmpty(model.Nome))
            {
                query = query.Where(x => x.Nome.ToUpper().Contains(model.Nome.ToUpper()));
            }

            if (model.PrecoInicial.HasValue)
            {
                query = query.Where(x => x.Preco >= model.PrecoInicial.Value);
            }

            if (model.PrecoFinal.HasValue)
            {
                query = query.Where(x => x.Preco <= model.PrecoFinal.Value);
            }

            if (model.CategoriaId.HasValue && model.CategoriaId > 0)
            {
                query = query.Where(x => x.CategoriaId == model.CategoriaId.Value);
            }

            // Mapeamento para a ViewModel de resultados
            model.Resultados = query.Select(p => new ProdutoCadastroViewModel 
            { 
                ProdutoId = p.ProdutoId,
                Nome = p.Nome,
                CodigoBarras = p.CodigoBarras,
                Preco = p.Preco,
                QuantidadeEstoque = p.QuantidadeEstoque,
                CategoriaId = p.CategoriaId,
                CategoriaNome = p.Categoria != null ? p.Categoria.Nome : "Sem Categoria"
            }).ToList();

            model.Categorias = ObterListaCategorias();

            return View(model);
        }

        // CADASTRO (GET)
        public IActionResult Create()
        {
            var model = new ProdutoCadastroViewModel
            {
                Categorias = ObterListaCategorias()
            };
            return View(model);
        }

        // CADASTRO (POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(ProdutoCadastroViewModel model)
        {
            if (ModelState.IsValid)
            {
                var produto = new Produto
                {
                    CodigoBarras = model.CodigoBarras,
                    Nome = model.Nome,
                    Descricao = model.Descricao,
                    CategoriaId = model.CategoriaId,
                    Preco = model.Preco,
                    QuantidadeEstoque = model.QuantidadeEstoque
                };

                _context.Produtos.Add(produto);
                _context.SaveChanges();

                return RedirectToAction(nameof(Index));
            }

            model.Categorias = ObterListaCategorias();
            return View(model);
        }

        // EDIÇÃO (GET)
        public IActionResult Edit(int id)
        {
            var produto = _context.Produtos.Find(id);
            if (produto == null) return NotFound();

            var model = new ProdutoCadastroViewModel
            {
                ProdutoId = produto.ProdutoId,
                CodigoBarras = produto.CodigoBarras,
                Nome = produto.Nome,
                Descricao = produto.Descricao,
                CategoriaId = produto.CategoriaId,
                Preco = produto.Preco,
                QuantidadeEstoque = produto.QuantidadeEstoque,
                Categorias = ObterListaCategorias()
            };

            return View(model);
        }

        // EDIÇÃO (POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(ProdutoCadastroViewModel model)
        {
            if (ModelState.IsValid)
            {
                var produto = _context.Produtos.Find(model.ProdutoId);
                if (produto == null) return NotFound();

                produto.CodigoBarras = model.CodigoBarras;
                produto.Nome = model.Nome;
                produto.Descricao = model.Descricao;
                produto.CategoriaId = model.CategoriaId;
                produto.Preco = model.Preco;
                produto.QuantidadeEstoque = model.QuantidadeEstoque;

                _context.Produtos.Update(produto);
                _context.SaveChanges();

                return RedirectToAction(nameof(Index));
            }

            model.Categorias = ObterListaCategorias();
            return View(model);
        }

        // EXCLUSÃO (GET)
        public IActionResult Delete(int id)
        {
            var produto = _context.Produtos.Include(p => p.Categoria).FirstOrDefault(p => p.ProdutoId == id);
            if (produto == null) return NotFound();

            var model = new ProdutoCadastroViewModel
            {
                ProdutoId = produto.ProdutoId,
                Nome = produto.Nome,
                Descricao = produto.Descricao,
                CodigoBarras = produto.CodigoBarras,
                CategoriaId = produto.CategoriaId,
                Preco = produto.Preco,
                QuantidadeEstoque = produto.QuantidadeEstoque
            };

            return View(model);
        }

        // EXCLUSÃO (POST) - AQUI ESTAVA O ERRO
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int ProdutoId) 
        {
            // Buscamos pelo ID que vem do campo hidden do formulário
            var produto = _context.Produtos.Find(ProdutoId);
            
            if (produto == null) return NotFound();

            _context.Produtos.Remove(produto);
            _context.SaveChanges();

            return RedirectToAction(nameof(Index));
        }

        // MÉTODO AUXILIAR PARA SELECT DE CATEGORIAS
        private List<SelectListItem> ObterListaCategorias()
        {
            return _context.Categorias
                .AsNoTracking() 
                .Select(x => new SelectListItem
                {
                    Value = x.CategoriaId.ToString(),
                    Text = x.Nome
                })
                .OrderBy(x => x.Text)
                .ToList();
        }
    }
}