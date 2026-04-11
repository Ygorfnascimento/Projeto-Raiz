using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Raiz.Data;
using Raiz.Models;
using Raiz.ViewModels; 

namespace Raiz.Controllers;

public class CategoriaController : Controller
{
    private readonly ApplicationDbContext _context;

    public CategoriaController(ApplicationDbContext context)
    {
        _context = context;
    }

    public IActionResult Index([FromQuery] CategoriaPesquisaViewModel model)
    {
        var query = _context.Categorias.AsNoTracking();

        if (!string.IsNullOrEmpty(model.Nome))
        {
            query = query.Where(x => x.Nome.Contains(model.Nome));
        }

        model.Resultados = query.Select(c => new CategoriaCadastroViewModel
        {
            CategoriaId = c.CategoriaId,
            Nome = c.Nome ?? ""
        }).ToList();

        return View(model);
    }

    public IActionResult Create()
    {
        return View(new CategoriaCadastroViewModel());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Create(CategoriaCadastroViewModel model)
    {
        if (ModelState.IsValid)
        {
            var categoria = new Categoria
            {
                Nome = model.Nome
            };

            _context.Categorias.Add(categoria);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        return View(model);
    }

    public IActionResult Edit(int id)
    {
        var categoria = _context.Categorias.Find(id);

        if (categoria == null)
            return NotFound();

        var model = new CategoriaCadastroViewModel
        {
            CategoriaId = categoria.CategoriaId,
            Nome = categoria.Nome ?? ""
        };

        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Edit(CategoriaCadastroViewModel model)
    {
        if (ModelState.IsValid)
        {
            var categoria = _context.Categorias.Find(model.CategoriaId);
            if (categoria == null) return NotFound();

            categoria.Nome = model.Nome;

            _context.Categorias.Update(categoria);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        return View(model);
    }
    
    public IActionResult Delete(int id)
    {
        var categoria = _context.Categorias.Find(id);

        if (categoria == null)
            return NotFound();

        var model = new CategoriaCadastroViewModel
        {
            CategoriaId = categoria.CategoriaId,
            Nome = categoria.Nome ?? ""
        };

        return View(model);
    }

    
    [HttpPost]
    [ValidateAntiForgeryToken]
    [ActionName("Delete")]
    public IActionResult DeleteConfirmed(int CategoriaId) 
    {
        var categoria = _context.Categorias.Find(CategoriaId);

        if (categoria == null)
            return NotFound();
            
        bool temProdutos = _context.Produtos.Any(p => p.CategoriaId == CategoriaId);
        if (temProdutos)
        {
            TempData["Erro"] = "Não é possível excluir esta categoria porque existem produtos vinculados a ela.";
            return RedirectToAction(nameof(Index));
        }

        _context.Categorias.Remove(categoria);
        _context.SaveChanges();
        return RedirectToAction(nameof(Index));
    }
}