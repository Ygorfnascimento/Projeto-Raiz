using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.SignalR;
using Raiz.Enums;
using Raiz.Models;

namespace Raiz.Models;

public class Movimentacao
{
    public Movimentacao()
    {
        StatusId = (int)Status.EmAndamento;
        Itens = new List<MovimentacaoItem>();
    }

public int MovimentacaoId { get; set;}

[Required]

public int TipoId {get; set;}

[Required]
[StringLength(25)]

public string Documento {get; set;} = string.Empty;

[Required]

public  DateTime DataMovimentacao {get; set;}

[Required]

<<<<<<< HEAD
public int StatusId {get;  private set;}
=======
public int StatusId {get; set;}
>>>>>>> e81c0a1 (refactor: refatoração geral dos controllers de Produto e Movimentação)

public bool PodeEditar
{
    get
        {
            return StatusId == (int)Status.EmAndamento;
        }    
}

public IEnumerable<MovimentacaoItem> Itens {get; set;}

public void Finalizar()
{
    foreach (var item in Itens)
        {
            item.Produto?.AtualizarEstoque((TipoMovimentacao)TipoId, item.Quantidade);
        }    

        StatusId = (int)Status.Finalizada;
}

}