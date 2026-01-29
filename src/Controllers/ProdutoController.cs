using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
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

        public IActionResult Index()
        {
            var produtos = _context.Produtos.ToList();
            return View(produtos);
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

        public IActionResult Edit (int id)
        {
            var produto = _context.Produtos.Find(id);

            if (produto == null)
            {
                return NotFound();

            }

            var model = new ProdutoRegister();

            model.Produto = produto;

            model.Categorias = model.Categorias = LoadDropdownlistCategorias();

            return View(model);
        } 

        [HttpPost]
        [ValidateAntiForgeryToken]

        public IActionResult Edit (Produto produto)
        {
            

            if (ModelState.IsValid)
            {
                _context.Produtos.Update(produto);
                _context.SaveChanges();

                return RedirectToAction(nameof(Index));
            }
            else 
            {
            var model = new ProdutoRegister();

            model.Categorias = model.Categorias = LoadDropdownlistCategorias();

            model.Produto = produto;

            return View(model);
            }

        }
        
        public IActionResult Delete (int id)
        {
            var produto = _context.Produtos.Find(id);

            if(produto == null)
            {
                return NotFound();
            }
            return View (produto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]

        public IActionResult Delete (Produto produto) 
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
