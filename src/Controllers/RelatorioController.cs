using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Raiz.Data;
using Raiz.ViewModels;
using Raiz.Enums;

namespace Raiz.Controllers
{
    [Authorize(Roles = "Gerente")]
    public class RelatorioController : Controller
    {
        private readonly ApplicationDbContext _context;

        public RelatorioController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var dashboard = new DashboardViewModel();
    
            dashboard.TotalProdutos = await _context.Produtos.CountAsync();

            dashboard.TotalCategorias = await _context.Categorias.CountAsync();

            dashboard.TotalEntradas = await _context.Movimentacoes
                .Where(m => m.TipoId == (int)TipoMovimentacao.Entrada && m.StatusId == (int)Status.Finalizada)
                .SelectMany(m => m.Itens)
                .SumAsync(i => (double)i.Quantidade * (double)i.Preco);

            dashboard.TotalSaidas = await _context.Movimentacoes
                .Where(m => m.TipoId == (int)TipoMovimentacao.Saida && m.StatusId == (int)Status.Finalizada)
                .SelectMany(m => m.Itens)
                .SumAsync(i => (double)i.Quantidade * (double)i.Preco);

            return View(dashboard);
        }

        public IActionResult ProdutosPorCategoria()
        {
            var produtos = _context.Produtos
                .Include(p => p.Categoria)
                .ToList();

            var dados = produtos
                .GroupBy(p => p.Categoria?.Nome ?? "Sem Categoria")
                .Select(g => new ProdutoPorCategoriaViewModel
                {
                    Categoria = g.Key,
                    QuantidadeProdutos = g.Count()
                })
                .ToList();

            return View(dados);
        }

        public IActionResult MovimentacoesResumo()
        {
            var movimentacoes = _context.Movimentacoes
                .Include(m => m.Itens)
                .ToList(); 

            var dados = movimentacoes
                .GroupBy(m => m.TipoId)
                .Select(g => new MovimentacaoResumoViewModel
                {
                    Tipo = Enum.GetName(typeof(TipoMovimentacao), (TipoMovimentacao)g.Key) ?? "Outros",
                    Total = g.SelectMany(m => m.Itens).Sum(i => i.Quantidade * i.Preco)
                })
                .ToList();

            return View(dados);
        }

        public IActionResult ProdutosMaisVendidos()
        {
            var itens = _context.MovimentacaoItens
                .Include(i => i.Produto)
                .Include(i => i.Movimentacao)
                .Where(i => i.Movimentacao != null && i.Movimentacao.TipoId == (int)TipoMovimentacao.Saida)
                .ToList();

            var dados = itens
                .GroupBy(i => i.Produto?.Nome ?? "Produto Indefinido")
                .Select(g => new ProdutoMaisVendidoViewModel
                {
                    Produto = g.Key,
                    Quantidade = g.Sum(x => x.Quantidade)
                })
                .OrderByDescending(x => x.Quantidade)
                .Take(10)
                .ToList();

            return View(dados);
        }
    }
}