using Microsoft.AspNetCore.Mvc.Rendering;
using Raiz.Models;

namespace Raiz.ViewModels;

public class MovimentacaoRegister
{
    public MovimentacaoRegister()
    {

        TiposMovimentacoes = new List<SelectListItem>();
        Movimentacao = new Movimentacao();
    }
    public Movimentacao Movimentacao {get; set;}

    public List<SelectListItem> TiposMovimentacoes {get; set;}
    
}

