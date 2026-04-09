using Microsoft.AspNetCore.Mvc;
using Raiz.Data;
using Raiz.Models;
<<<<<<< HEAD
=======
using Raiz.ViewModels;
using Microsoft.EntityFrameworkCore;
>>>>>>> e81c0a1 (refactor: refatoração geral dos controllers de Produto e Movimentação)

namespace Raiz.Controllers;

public class CategoriaController : Controller
{
    private readonly ApplicationDbContext _context;

    public CategoriaController(ApplicationDbContext context)
    {
        _context = context;
    }

<<<<<<< HEAD
    public IActionResult Index()
    {
        var categorias = _context.Categorias.ToList();
        return View(categorias);
    }

    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Create(Categoria categoria)
    {
        if (ModelState.IsValid)
        {
=======
    public IActionResult Index([FromQuery] CategoriaPesquisaViewModel model)
    {
        var query = _context.Categorias.AsNoTracking();

        if (!string.IsNullOrEmpty(model.Nome))
        {
            query = query.Where(x => x.Nome.ToUpper().Contains(model.Nome.ToUpper()));
        }

        model.Resultados = query.Select(x => new CategoriaCadastroViewModel
        {
            CategoriaId = x.CategoriaId,
            Nome = x.Nome
        }).ToList();

        return View(model);
    }

    public IActionResult Create() => View();

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Create(CategoriaCadastroViewModel model)
    {
        if (ModelState.IsValid)
        {
            var categoria = new Categoria { Nome = model.Nome };
>>>>>>> e81c0a1 (refactor: refatoração geral dos controllers de Produto e Movimentação)
            _context.Categorias.Add(categoria);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
<<<<<<< HEAD

        return View(categoria);
=======
        return View(model);
>>>>>>> e81c0a1 (refactor: refatoração geral dos controllers de Produto e Movimentação)
    }

    public IActionResult Edit(int id)
    {
        var categoria = _context.Categorias.Find(id);
<<<<<<< HEAD

        if (categoria == null)
            return NotFound();

        return View(categoria);
=======
        if (categoria == null) return NotFound();

        return View(new CategoriaCadastroViewModel { 
            CategoriaId = categoria.CategoriaId, 
            Nome = categoria.Nome 
        });
>>>>>>> e81c0a1 (refactor: refatoração geral dos controllers de Produto e Movimentação)
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
<<<<<<< HEAD
    public IActionResult Edit(Categoria categoria)
    {
        if (ModelState.IsValid)
        {
=======
    public IActionResult Edit(CategoriaCadastroViewModel model)
    {
        if (ModelState.IsValid)
        {
            var categoria = _context.Categorias.Find(model.CategoriaId);
            if (categoria == null) return NotFound();

            categoria.Nome = model.Nome;
>>>>>>> e81c0a1 (refactor: refatoração geral dos controllers de Produto e Movimentação)
            _context.Categorias.Update(categoria);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
<<<<<<< HEAD

        return View(categoria);
    }
    
    public IActionResult Delete(int id)
    {
        var categoria = _context.Categorias.Find(id);

        if (categoria == null)
            return NotFound();

        return View(categoria);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [ActionName("Delete")]
    public IActionResult DeleteConfirmed(int id)
    {
        var categoria = _context.Categorias.Find(id);

        if (categoria == null)
            return NotFound();
=======
        return View(model);
    }

    [HttpGet]
    public IActionResult Delete(int id)
    {
        var categoria = _context.Categorias.Find(id);
        if (categoria == null) return NotFound();

        return View(new CategoriaCadastroViewModel { 
            CategoriaId = categoria.CategoriaId, 
            Nome = categoria.Nome 
        });
    }

    [HttpPost, ActionName("Delete")] 
    [ValidateAntiForgeryToken]
    public IActionResult DeleteConfirmed(int id)
    {
        var categoria = _context.Categorias.Find(id);
        if (categoria == null) return NotFound();

        var temProdutos = _context.Produtos.Any(p => p.CategoriaId == id);
        if (temProdutos)
        {
            TempData["Erro"] = "Não é possível excluir: existem produtos vinculados a esta categoria.";
            return RedirectToAction(nameof(Index));
        }
>>>>>>> e81c0a1 (refactor: refatoração geral dos controllers de Produto e Movimentação)

        _context.Categorias.Remove(categoria);
        _context.SaveChanges();
        return RedirectToAction(nameof(Index));
    }
<<<<<<< HEAD
}
=======
}
>>>>>>> e81c0a1 (refactor: refatoração geral dos controllers de Produto e Movimentação)
