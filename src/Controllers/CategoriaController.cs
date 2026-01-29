using Microsoft.AspNetCore.Mvc;
using Raiz.Data;
using Raiz.Models;

namespace Raiz.Controllers;

public class CategoriaController : Controller
{
    private readonly ApplicationDbContext _context;

    public CategoriaController(ApplicationDbContext context)
    {
        _context = context;
    }

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
            _context.Categorias.Add(categoria);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        return View(categoria);
    }

    public IActionResult Edit(int id)
    {
        var categoria = _context.Categorias.Find(id);

        if (categoria == null)
            return NotFound();

        return View(categoria);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Edit(Categoria categoria)
    {
        if (ModelState.IsValid)
        {
            _context.Categorias.Update(categoria);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

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

        _context.Categorias.Remove(categoria);
        _context.SaveChanges();
        return RedirectToAction(nameof(Index));
    }
}
