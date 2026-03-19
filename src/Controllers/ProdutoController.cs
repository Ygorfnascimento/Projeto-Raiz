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
        }

        public IActionResult Create()
        {
            var model = new ProdutoRegister();
            model.Categorias = LoadDropdownlistCategorias();
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(ProdutoRegister model)
        {
            if (ModelState.IsValid)
            {
                _context.Produtos.Add(model.Produto);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }

            model.Categorias = LoadDropdownlistCategorias();
            return View(model);
        }

        public IActionResult Edit(int id)
        {
            var produto = _context.Produtos.Find(id);

            if (produto == null)
            {
                return NotFound();
            }

            var model = new ProdutoRegister
            {
                Produto = produto,
                Categorias = LoadDropdownlistCategorias()
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
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

            return View(model);
        }

        public IActionResult Delete(int id)
        {
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
            _context.Produtos.Remove(produto);
            _context.SaveChanges();

            return RedirectToAction(nameof(Index));
        }

        private List<SelectListItem> LoadDropdownlistCategorias()
        {
            return _context.Categorias
                .Select(x => new SelectListItem
                {
                    Value = x.CategoriaId.ToString(),
                    Text = x.Nome
                })
                .ToList();
        }
    }
}