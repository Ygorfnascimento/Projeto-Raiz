using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Raiz.Data;
using Raiz.Enums;
using Raiz.Models;
using Raiz.ViewModels;

namespace Raiz.Controllers;

public class MovimentacaoController : Controller
{
    private readonly ApplicationDbContext _context;

    public MovimentacaoController(ApplicationDbContext context)
    {
        _context = context;
    }

    public IActionResult Index([FromQuery] MovimentacaoSearch model)
    {
        var query = _context.Movimentacoes.AsNoTracking();

        if (model.MovimentacaoId.HasValue && model.MovimentacaoId > 0)
            query = query.Where(x => x.MovimentacaoId == model.MovimentacaoId);

        if (!string.IsNullOrEmpty(model.Documento))
            query = query.Where(x => x.Documento.Contains(model.Documento));

        if (model.TipoId.HasValue && model.TipoId > 0)
            query = query.Where(x => x.TipoId == model.TipoId);

        if (model.StatusId.HasValue && model.StatusId > 0)
            query = query.Where(x => x.StatusId == model.StatusId);

        if (model.DataMovimentacaoInicial.HasValue)
            query = query.Where(x => x.DataMovimentacao >= model.DataMovimentacaoInicial);

        if (model.DataMovimentacaoFinal.HasValue)
            query = query.Where(x => x.DataMovimentacao <= model.DataMovimentacaoFinal);

        model.Resultado = query.ToList();
        model.TiposMovimentacoes = LoadDropdownlistTiposMovimentacoes();
        model.Status = LoadDropdownlistStatus();

        return View(model);
    }

    public IActionResult Create()
    {
        return View(new MovimentacaoRegister
        {
            Movimentacao = new Movimentacao
            {
                DataMovimentacao = DateTime.Now
            },
            TiposMovimentacoes = LoadDropdownlistTiposMovimentacoes()
        });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Create(Movimentacao movimentacao)
    {
        if (!ModelState.IsValid)
        {
            return View(new MovimentacaoRegister
            {
                Movimentacao = movimentacao,
                TiposMovimentacoes = LoadDropdownlistTiposMovimentacoes()
            });
        }

        _context.Movimentacoes.Add(movimentacao);
        _context.SaveChanges();

        return RedirectToAction(nameof(Edit), new { id = movimentacao.MovimentacaoId });
    }

    public IActionResult Edit(int id)
    {
        var movimentacao = _context.Movimentacoes
            .Include(i => i.Itens)
            .ThenInclude(p => p.Produto)
            .FirstOrDefault(m => m.MovimentacaoId == id);

        if (movimentacao == null)
            return NotFound();

        if (!movimentacao.PodeEditar)
            return Forbid();

        return View(new MovimentacaoRegister
        {
            Movimentacao = movimentacao
        });
    }

    [HttpPost]
    public IActionResult AddItem(MovimentacaoItem item)
    {
        _context.MovimentacaoItens.Add(item);
        _context.SaveChanges();

        return RedirectToAction(nameof(Edit), new { id = item.MovimentacaoId });
    }

    [HttpGet]
    public IActionResult DeleteItem(int id)
    {
        var item = _context.MovimentacaoItens.Find(id);

        if (item != null)
        {
            var movimentacaoId = item.MovimentacaoId;

            _context.MovimentacaoItens.Remove(item);
            _context.SaveChanges();

            return RedirectToAction(nameof(Edit), new { id = movimentacaoId });
        }

        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Finalize(MovimentacaoRegister movimentacaoRegister)
    {
        try
        {
            var model = _context.Movimentacoes
                .Include(i => i.Itens)
                .ThenInclude(p => p.Produto)
                .FirstOrDefault(m => m.MovimentacaoId == movimentacaoRegister.Movimentacao.MovimentacaoId);

            if (model == null)
                return NotFound();

            if (!model.PodeEditar)
                return Forbid();

            model.Finalizar();
            _context.SaveChanges();

            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            ViewBag.Mensagem = ex.Message;

            return RedirectToAction(nameof(Edit), new
            {
                id = movimentacaoRegister.Movimentacao.MovimentacaoId
            });
        }
    }

    public IActionResult Detail(int id)
    {
        var movimentacao = _context.Movimentacoes
            .Include(i => i.Itens)
            .ThenInclude(p => p.Produto)
            .FirstOrDefault(m => m.MovimentacaoId == id);

        if (movimentacao == null)
            return NotFound();

        return View(new MovimentacaoRegister
        {
            Movimentacao = movimentacao
        });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Delete(int id)
    {
        var movimentacao = _context.Movimentacoes
            .Include(m => m.Itens)
            .FirstOrDefault(m => m.MovimentacaoId == id);

        if (movimentacao == null)
            return NotFound();

        _context.MovimentacaoItens.RemoveRange(movimentacao.Itens);

        _context.Movimentacoes.Remove(movimentacao);
        _context.SaveChanges();

        return RedirectToAction(nameof(Index));
    }

    private List<SelectListItem> LoadDropdownlistTiposMovimentacoes()
    {
        return new List<SelectListItem>
        {
            new SelectListItem
            {
                Value = ((int)TipoMovimentacao.Entrada).ToString(),
                Text = TipoMovimentacao.Entrada.ToString()
            },
            new SelectListItem
            {
                Value = ((int)TipoMovimentacao.Saida).ToString(),
                Text = TipoMovimentacao.Saida.ToString()
            }
        };
    }

    private List<SelectListItem> LoadDropdownlistStatus()
    {
        return new List<SelectListItem>
        {
            new SelectListItem
            {
                Value = ((int)Status.EmAndamento).ToString(),
                Text = Status.EmAndamento.ToString()
            },
            new SelectListItem
            {
                Value = ((int)Status.Finalizada).ToString(),
                Text = Status.Finalizada.ToString()
            }
        };
    }
}