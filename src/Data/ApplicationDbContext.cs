using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Raiz.Models;

namespace Raiz.Data;

public class ApplicationDbContext : IdentityDbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<Categoria> Categorias { get; set; }

    public DbSet<Produto> Produtos { get; set; }

    public DbSet<Movimentacao> Movimentacoes { get; set; }

    public DbSet<MovimentacaoItem> MovimentacaoItens { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<MovimentacaoItem>()
            .HasOne(mi => mi.Movimentacao)
            .WithMany(m => m.Itens)
            .HasForeignKey(mi => mi.MovimentacaoId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<MovimentacaoItem>()
            .HasOne(mi => mi.Produto)
            .WithMany()
            .HasForeignKey(mi => mi.ProdutoId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}