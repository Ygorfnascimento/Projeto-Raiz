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

<<<<<<< HEAD
        public IActionResult Index([FromQuery] ProdutoSearch model)
        {
            var query  = _context.Produtos
                .Include(p => p.Categoria)
                .AsNoTracking();

            if(model.ProdutoId.HasValue && model.ProdutoId > 0)
                query = query.Where(p => p.ProdutoId == model.ProdutoId);

            if(!string.IsNullOrEmpty(model.Nome))
                query = query.Where(x => x.Nome.ToUpper().Contains(model.Nome.ToUpper()));

            if(model.CategoriaId.HasValue && model.CategoriaId > 0)
                query = query.Where(x => x.CategoriaId == model.CategoriaId);

            if(model.PrecoInicial.HasValue && model.PrecoInicial > 0)
                query = query.Where(x => x.Preco >= model.PrecoInicial);

            if(model.PrecoFinal.HasValue && model.PrecoFinal > 0)
                query = query.Where(x => x.Preco <= model.PrecoFinal);

             // model.Resultado = _context.Produtos
             //   .Include(p => p.Categoria)
             //   .ToList();

            model.Resultado = query.ToList();

            model.CategoriasSelect = LoadDropdownlistCategorias();

            return View(nameof(Index), model);
=======
        [HttpGet]
        public IActionResult Index([FromQuery] ProdutoPesquisaViewModel model)
        {
            var query = _context.Produtos.Include(p => p.Categoria).AsNoTracking();

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
                query = query.Where(x => x.Preco >= (decimal)model.PrecoInicial.Value);
            }

            if (model.PrecoFinal.HasValue)
            {
                query = query.Where(x => x.Preco <= (decimal)model.PrecoFinal.Value);
            }

            if (model.CategoriaId.HasValue && model.CategoriaId > 0)
            {
                query = query.Where(x => x.CategoriaId == model.CategoriaId.Value);
            }

            model.Resultados = query.Select(p => new ProdutoCadastroViewModel 
            { 
                ProdutoId = p.ProdutoId,
                Nome = p.Nome,
                CodigoBarras = p.CodigoBarras,
                Preco = (decimal)p.Preco,
                QuantidadeEstoque = p.QuantidadeEstoque,
                CategoriaId = p.CategoriaId,
                CategoriaNome = p.Categoria != null ? p.Categoria.Nome : "Sem Categoria"
            }).ToList();

            model.Categorias = ObterListaCategorias();

            return View(model);
>>>>>>> e81c0a1 (refactor: refatoração geral dos controllers de Produto e Movimentação)
        }

        public IActionResult Create()
        {
<<<<<<< HEAD
            var model = new ProdutoRegister();
            model.Categorias = LoadDropdownlistCategorias();
=======
            var model = new ProdutoCadastroViewModel
            {
                Categorias = ObterListaCategorias()
            };
>>>>>>> e81c0a1 (refactor: refatoração geral dos controllers de Produto e Movimentação)
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
<<<<<<< HEAD
        public IActionResult Create(ProdutoRegister model)
        {
            if (ModelState.IsValid)
            {
                _context.Produtos.Add(model.Produto);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }

            model.Categorias = LoadDropdownlistCategorias();
=======
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
                    Preco = (decimal)model.Preco,
                    QuantidadeEstoque = (int)model.QuantidadeEstoque
                };

                _context.Produtos.Add(produto);
                _context.SaveChanges();

                return RedirectToAction(nameof(Index));
            }

            model.Categorias = ObterListaCategorias();
>>>>>>> e81c0a1 (refactor: refatoração geral dos controllers de Produto e Movimentação)
            return View(model);
        }

        public IActionResult Edit(int id)
        {
            var produto = _context.Produtos.Find(id);
<<<<<<< HEAD

            if (produto == null)
            {
                return NotFound();
            }

            var model = new ProdutoRegister
            {
                Produto = produto,
                Categorias = LoadDropdownlistCategorias()
=======
            if (produto == null) return NotFound();

            var model = new ProdutoCadastroViewModel
            {
                ProdutoId = produto.ProdutoId,
                CodigoBarras = produto.CodigoBarras,
                Nome = produto.Nome,
                Descricao = produto.Descricao,
                CategoriaId = produto.CategoriaId,
                Preco = (decimal)produto.Preco,
                QuantidadeEstoque = produto.QuantidadeEstoque,
                Categorias = ObterListaCategorias()
>>>>>>> e81c0a1 (refactor: refatoração geral dos controllers de Produto e Movimentação)
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
<<<<<<< HEAD
        public IActionResult Edit(Produto produto)
        {
            if (ModelState.IsValid)
            {
                _context.Produtos.Update(produto);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }

            var model = new ProdutoRegister
            {
                Produto = produto,
                Categorias = LoadDropdownlistCategorias()
            };

=======
        public IActionResult Edit(ProdutoCadastroViewModel model)
        {
            if (model.CategoriaId <= 0)
            {
                ModelState.AddModelError("CategoriaId", "Selecione uma categoria válida.");
            }

            if (ModelState.IsValid)
            {
                var produto = _context.Produtos.Find(model.ProdutoId);
                if (produto == null) return NotFound();

                produto.CodigoBarras = model.CodigoBarras;
                produto.Nome = model.Nome;
                produto.Descricao = model.Descricao;
                produto.CategoriaId = model.CategoriaId;
                produto.Preco = (decimal)model.Preco;
                produto.QuantidadeEstoque = (int)model.QuantidadeEstoque;

                _context.Produtos.Update(produto);
                _context.SaveChanges();

                return RedirectToAction(nameof(Index));
            }

            model.Categorias = ObterListaCategorias();
>>>>>>> e81c0a1 (refactor: refatoração geral dos controllers de Produto e Movimentação)
            return View(model);
        }

        public IActionResult Delete(int id)
        {
<<<<<<< HEAD
            var produto = _context.Produtos.Find(id);

            if (produto == null)
            {
                return NotFound();
            }

            return View(produto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(Produto produto)
        {
=======
            var produto = _context.Produtos.Include(p => p.Categoria).FirstOrDefault(p => p.ProdutoId == id);
            if (produto == null) return NotFound();

            var model = new ProdutoCadastroViewModel
            {
                ProdutoId = produto.ProdutoId,
                Nome = produto.Nome,
                Descricao = produto.Descricao,
                CodigoBarras = produto.CodigoBarras,
                CategoriaId = produto.CategoriaId,
                Preco = (decimal)produto.Preco,
                QuantidadeEstoque = produto.QuantidadeEstoque
            };

            return View(model);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int ProdutoId) 
        {
            var produto = _context.Produtos.Find(ProdutoId);
            if (produto == null) return NotFound();

>>>>>>> e81c0a1 (refactor: refatoração geral dos controllers de Produto e Movimentação)
            _context.Produtos.Remove(produto);
            _context.SaveChanges();

            return RedirectToAction(nameof(Index));
        }

<<<<<<< HEAD
        private List<SelectListItem> LoadDropdownlistCategorias()
        {
            return _context.Categorias
=======
        private List<SelectListItem> ObterListaCategorias()
        {
            return _context.Categorias
                .AsNoTracking() 
>>>>>>> e81c0a1 (refactor: refatoração geral dos controllers de Produto e Movimentação)
                .Select(x => new SelectListItem
                {
                    Value = x.CategoriaId.ToString(),
                    Text = x.Nome
                })
<<<<<<< HEAD
=======
                .OrderBy(x => x.Text)
>>>>>>> e81c0a1 (refactor: refatoração geral dos controllers de Produto e Movimentação)
                .ToList();
        }
    }
}