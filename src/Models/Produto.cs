using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
<<<<<<< HEAD
using Raiz.Enums;

namespace Raiz.Models;

public class Produto
{
    [DisplayName("Id")]
    public int ProdutoId {get; set;}

    [Required]
    [StringLength(100)]

    public string Nome {get; set;} = string.Empty;

    [Required]
    [StringLength(100)]
    [DisplayName("Descrição")]

    public string Descricao {get; set;} = string.Empty;
    [DisplayName("Categoria")]
    public int CategoriaId {get; set;}
    [DisplayName("Preço")]
    public double Preco {get; set;}

    public double QuantidadeEstoque { get; private set;}

    public Categoria? Categoria {get; set;}


    public bool TemEstoque (double quantidade)
    {
        return QuantidadeEstoque >= quantidade;
    }

    public void AtualizarEstoque(TipoMovimentacao tipoMovimentacao, double quantidade)
    {
        
        if(quantidade <= 0)
            throw new Exception("Quantidade deve ser maior que zero");

        if(tipoMovimentacao == TipoMovimentacao.Saida && !TemEstoque(quantidade))
            throw new Exception ("Estoque insuficiente");

        if(tipoMovimentacao == TipoMovimentacao.Entrada)
        {
            QuantidadeEstoque += quantidade;
        }
        else
        {
            QuantidadeEstoque -= quantidade;
        }
    }
}

        
=======
using System.ComponentModel.DataAnnotations.Schema;
using Raiz.Enums;

namespace Raiz.Models
{
    public class Produto
    {
        [DisplayName("Id")]
        public int ProdutoId { get; set; }

        [Required]
        [StringLength(100)]
        public string CodigoBarras { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        public string Nome { get; set; } = string.Empty;

        [DisplayName("Descrição")]
        public string Descricao { get; set; } = string.Empty;

        [DisplayName("Categoria")]
        public int CategoriaId { get; set; }

        [DisplayName("Preço")]
        public decimal Preco { get; set; }

        public int QuantidadeEstoque { get; set; } 

        public Categoria? Categoria { get; set; }

        public bool TemEstoque(int quantidade)
        {
            return QuantidadeEstoque >= quantidade;
        }

        public void AtualizarEstoque(TipoMovimentacao tipoMovimentacao, int quantidade)
        {
            if (quantidade <= 0)
                throw new Exception("Quantidade deve ser maior que zero");

            if (tipoMovimentacao == TipoMovimentacao.Saida && !TemEstoque(quantidade))
                throw new Exception("Estoque insuficiente");

            if (tipoMovimentacao == TipoMovimentacao.Entrada)
                QuantidadeEstoque += quantidade;
            else
                QuantidadeEstoque -= quantidade;
        }
    }
}
>>>>>>> e81c0a1 (refactor: refatoração geral dos controllers de Produto e Movimentação)
