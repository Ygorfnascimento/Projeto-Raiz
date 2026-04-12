using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Raiz.Data;
using Raiz.Enums;
using Raiz.Models;
using Raiz.ViewModels;

namespace Raiz.Controllers
{
    [Authorize]
    public class MovimentacaoController : Controller
    {
        private readonly ApplicationDbContext _context;

        public MovimentacaoController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Index([FromQuery] MovimentacaoPesquisaViewModel model)
        {
            var query = _context.Movimentacoes.AsNoTracking();

            if (model.MovimentacaoId.HasValue)
                query = query.Where(x => x.MovimentacaoId == model.MovimentacaoId);

            if (!string.IsNullOrEmpty(model.Documento))
                query = query.Where(x => x.Documento.Contains(model.Documento));

            if (model.TipoId.HasValue)
                query = query.Where(x => x.TipoId == model.TipoId);

            if (model.StatusId.HasValue)
                query = query.Where(x => x.StatusId == model.StatusId);

            if (model.DataMovimentacaoInicial.HasValue)
                query = query.Where(x => x.DataMovimentacao >= model.DataMovimentacaoInicial);

            if (model.DataMovimentacaoFinal.HasValue)
                query = query.Where(x => x.DataMovimentacao <= model.DataMovimentacaoFinal);

            var lista = query
                .Select(m => new { m.MovimentacaoId, m.Documento, m.DataMovimentacao, m.StatusId, m.TipoId })
                .ToList();

            model.Resultado = lista.Select(m => new MovimentacaoCadastroViewModel
            {
                MovimentacaoId = m.MovimentacaoId,
                Documento = m.Documento,
                DataMovimentacao = m.DataMovimentacao,
                StatusId = m.StatusId,
                StatusNome = Enum.GetName(typeof(Status), m.StatusId) ?? "Indefinido",
                TipoId = m.TipoId,
                TipoNome = Enum.GetName(typeof(TipoMovimentacao), m.TipoId) ?? "Indefinido"
            }).ToList();

            model.TiposMovimentacoes = ObterListaTiposMovimentacoes();
            model.Status = ObterListaStatus();

            return View(model);
        }

        public IActionResult Create()
        {
            var model = new MovimentacaoCadastroViewModel
            {
                DataMovimentacao = DateTime.Now,
                TiposMovimentacoes = ObterListaTiposMovimentacoes()
            };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(MovimentacaoCadastroViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model.TiposMovimentacoes = ObterListaTiposMovimentacoes();
                return View(model);
            }

            var movimentacao = new Movimentacao
            {
                DataMovimentacao = model.DataMovimentacao,
                TipoId = model.TipoId,
                Documento = model.Documento,
                StatusId = (int)Status.EmAndamento
            };

            _context.Movimentacoes.Add(movimentacao);
            _context.SaveChanges();

            return RedirectToAction(nameof(Edit), new { id = movimentacao.MovimentacaoId });
        }

        public IActionResult Detail(int id)
        {
            var movimentacao = _context.Movimentacoes
                .Include(m => m.Itens)
                .ThenInclude(i => i.Produto)
                .FirstOrDefault(m => m.MovimentacaoId == id);

            if (movimentacao == null) return NotFound();

            var model = new MovimentacaoCadastroViewModel
            {
                MovimentacaoId = movimentacao.MovimentacaoId,
                Documento = movimentacao.Documento,
                DataMovimentacao = movimentacao.DataMovimentacao,
                StatusId = movimentacao.StatusId,
                TipoId = movimentacao.TipoId,
                StatusNome = Enum.GetName(typeof(Status), movimentacao.StatusId) ?? "",
                TipoNome = Enum.GetName(typeof(TipoMovimentacao), movimentacao.TipoId) ?? "",
                Itens = movimentacao.Itens.Select(i => new MovimentacaoItemCadastroViewModel
                {
                    MovimentacaoId = i.MovimentacaoId,
                    MovimentacaoItemId = i.MovimentacaoItemId,
                    CodigoBarras = i.Produto?.CodigoBarras ?? "",
                    ProdutoNome = i.Produto?.Nome ?? "Produto não encontrado",
                    Preco = i.Preco,
                    Quantidade = i.Quantidade
                }).ToList(),
            };
            return View(model);
        }

        public IActionResult Edit(int id)
        {
            var movimentacao = _context.Movimentacoes
                .Include(m => m.Itens)
                .ThenInclude(i => i.Produto)
                .FirstOrDefault(m => m.MovimentacaoId == id);

            if (movimentacao == null) return NotFound();

            if (movimentacao.StatusId == (int)Status.Finalizada)
            {
                return RedirectToAction(nameof(Detail), new { id = id });
            }

            var model = new MovimentacaoCadastroViewModel
            {
                MovimentacaoId = movimentacao.MovimentacaoId,
                Documento = movimentacao.Documento,
                DataMovimentacao = movimentacao.DataMovimentacao,
                StatusId = movimentacao.StatusId,
                TipoId = movimentacao.TipoId,
                StatusNome = Enum.GetName(typeof(Status), movimentacao.StatusId) ?? "",
                TipoNome = Enum.GetName(typeof(TipoMovimentacao), movimentacao.TipoId) ?? "",
                Itens = movimentacao.Itens.Select(i => new MovimentacaoItemCadastroViewModel
                {
                    MovimentacaoId = i.MovimentacaoId,
                    MovimentacaoItemId = i.MovimentacaoItemId,
                    CodigoBarras = i.Produto?.CodigoBarras ?? "",
                    ProdutoNome = i.Produto?.Nome ?? "Produto não encontrado",
                    Preco = i.Preco,
                    Quantidade = i.Quantidade
                }).ToList(),
            };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Gerente")]
        public IActionResult Delete(int id)
        {
            var movimentacao = _context.Movimentacoes
                .Include(m => m.Itens)
                .FirstOrDefault(m => m.MovimentacaoId == id);

            if (movimentacao == null) return NotFound();

            if (movimentacao.StatusId == (int)Status.Finalizada)
            {
                TempData["Erro"] = "Não é possível excluir uma movimentação finalizada.";
                return RedirectToAction(nameof(Index));
            }

            _context.MovimentacaoItens.RemoveRange(movimentacao.Itens);
            _context.Movimentacoes.Remove(movimentacao);
            _context.SaveChanges();

            TempData["Mensagem"] = "Movimentação excluída com sucesso.";
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddItem(MovimentacaoItemCadastroViewModel model)
        {
            var produto = _context.Produtos.FirstOrDefault(x => x.CodigoBarras == model.CodigoBarras);

            if (string.IsNullOrEmpty(model.CodigoBarras) || model.Quantidade <= 0)
            {
                TempData["Erro"] = "Dados inválidos.";
                return RedirectToAction(nameof(Edit), new { id = model.MovimentacaoId });
            }

            if (produto == null)
            {
                TempData["Erro"] = "Produto não encontrado.";
                return RedirectToAction(nameof(Edit), new { id = model.MovimentacaoId });
            }

            var movimentacao = _context.Movimentacoes.Find(model.MovimentacaoId);
            if (movimentacao == null || movimentacao.StatusId == (int)Status.Finalizada) return Forbid();

            var item = new MovimentacaoItem
            {
                MovimentacaoId = model.MovimentacaoId,
                ProdutoId = produto.ProdutoId,
                Preco = model.Preco,
                Quantidade = model.Quantidade
            };

            _context.MovimentacaoItens.Add(item);
            _context.SaveChanges();

            return RedirectToAction(nameof(Edit), new { id = item.MovimentacaoId });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteItem(int id)
        {
            var movimentacaoItem = _context.MovimentacaoItens
                .Include(i => i.Movimentacao)
                .FirstOrDefault(i => i.MovimentacaoItemId == id);

            if (movimentacaoItem == null) return NotFound();

            if (movimentacaoItem.Movimentacao!.StatusId == (int)Status.Finalizada)
            {
                TempData["Erro"] = "Nota fechada não pode ser alterada.";
                return RedirectToAction(nameof(Detail), new { id = movimentacaoItem.MovimentacaoId });
            }

            int movimentacaoId = movimentacaoItem.MovimentacaoId;
            _context.MovimentacaoItens.Remove(movimentacaoItem);
            _context.SaveChanges();

            return RedirectToAction(nameof(Edit), new { id = movimentacaoId });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Finalize(MovimentacaoCadastroViewModel model)
        {
            try
            {
                var movimentacao = _context.Movimentacoes
                    .Include(m => m.Itens)
                    .ThenInclude(i => i.Produto)
                    .FirstOrDefault(m => m.MovimentacaoId == model.MovimentacaoId);

                if (movimentacao == null) return NotFound();
                if (movimentacao.StatusId == (int)Status.Finalizada) return Forbid();
                if (!movimentacao.Itens.Any())
                {
                    TempData["Erro"] = "Adicione itens antes de finalizar.";
                    return RedirectToAction(nameof(Edit), new { id = model.MovimentacaoId });
                }

                movimentacao.Finalizar();
                _context.SaveChanges();

                TempData["Mensagem"] = "Movimentação finalizada com sucesso!";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["Erro"] = ex.Message;
                return RedirectToAction(nameof(Edit), new { id = model.MovimentacaoId });
            }
        }

        #region Auxiliares
        private List<SelectListItem> ObterListaTiposMovimentacoes() => new() {
            new SelectListItem { Value = "1", Text = "Entrada" },
            new SelectListItem { Value = "2", Text = "Saída" }
        };

        private List<SelectListItem> ObterListaStatus() => new() {
            new SelectListItem { Value = "1", Text = "Em Andamento" },
            new SelectListItem { Value = "2", Text = "Finalizada" }
        };
        #endregion
    }
}