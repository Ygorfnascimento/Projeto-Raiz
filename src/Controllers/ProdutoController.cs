using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Raiz.Data;
using Raiz.Models;
using Raiz.ViewModels;

namespace Raiz.Controllers
{
    [Authorize] 
    public class ProdutoController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ProdutoController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Index([FromQuery] ProdutoPesquisaViewModel model)
        {
            var query = _context.Produtos.Include(p => p.Categoria).AsNoTracking();

            if (!string.IsNullOrEmpty(model.CodigoBarras))
                query = query.Where(x => x.CodigoBarras == model.CodigoBarras);

            if (!string.IsNullOrEmpty(model.Nome))
                query = query.Where(x => x.Nome.ToUpper().Contains(model.Nome.ToUpper()));

            if (model.PrecoInicial.HasValue)
                query = query.Where(x => x.Preco >= model.PrecoInicial.Value);

            if (model.PrecoFinal.HasValue)
                query = query.Where(x => x.Preco <= model.PrecoFinal.Value);

            if (model.CategoriaId.HasValue && model.CategoriaId > 0)
                query = query.Where(x => x.CategoriaId == model.CategoriaId.Value);

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

        [Authorize(Roles = "Gerente")]
        public IActionResult Create()
        {
            var model = new ProdutoCadastroViewModel { Categorias = ObterListaCategorias() };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Gerente")]
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
                TempData["Mensagem"] = "Produto cadastrado com sucesso!";
                return RedirectToAction(nameof(Index));
            }
            model.Categorias = ObterListaCategorias();
            return View(model);
        }

        [Authorize(Roles = "Gerente")]
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

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Gerente")]
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
                TempData["Mensagem"] = "Produto atualizado com sucesso!";
                return RedirectToAction(nameof(Index));
            }
            model.Categorias = ObterListaCategorias();
            return View(model);
        }

        [Authorize(Roles = "Gerente")]
        public IActionResult Delete(int id)
        {
            var produto = _context.Produtos.Include(p => p.Categoria).FirstOrDefault(p => p.ProdutoId == id);
            if (produto == null) return NotFound();

            var model = new ProdutoCadastroViewModel
            {
                ProdutoId = produto.ProdutoId,
                Nome = produto.Nome,
                CategoriaNome = produto.Categoria?.Nome ?? "Sem Categoria",
                Preco = produto.Preco,
                QuantidadeEstoque = produto.QuantidadeEstoque
            };
            return View(model);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Gerente")]
        public IActionResult DeleteConfirmed(int ProdutoId)
        {
            var produto = _context.Produtos.Find(ProdutoId);
            if (produto == null) return NotFound();

            var possuiMovimentacao = _context.MovimentacaoItens.Any(i => i.ProdutoId == ProdutoId);
            if (possuiMovimentacao)
            {
                TempData["Erro"] = "Impossível excluir: este produto possui histórico de movimentação.";
                return RedirectToAction(nameof(Index));
            }

            _context.Produtos.Remove(produto);
            _context.SaveChanges();
            TempData["Mensagem"] = "Produto removido com sucesso.";
            return RedirectToAction(nameof(Index));
        }

        private List<SelectListItem> ObterListaCategorias()
        {
            return _context.Categorias.AsNoTracking()
                .OrderBy(x => x.Nome)
                .Select(x => new SelectListItem { Value = x.CategoriaId.ToString(), Text = x.Nome })
                .ToList();
        }
    }
}