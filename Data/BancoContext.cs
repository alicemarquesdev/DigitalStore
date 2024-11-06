using DigitalStore.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;

namespace DigitalStore.Data
{
    public class BancoContext : DbContext
    {
        public BancoContext(DbContextOptions<BancoContext> options) : base(options)
        {
        }

        public DbSet<UsuarioModel> Usuarios { get; set; }

        public DbSet<ProdutoModel> Produtos { get; set; }

        public DbSet<CarrinhoModel> Carrinho { get; set; }

        public DbSet<FavoritosModel> Favoritos { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configurar o tipo de coluna da propriedade 'Preco' com precisão e escala
            modelBuilder.Entity<ProdutoModel>()
                .Property(p => p.Preco)
                .HasColumnType("decimal(18,2)");  // Exemplo: 18 dígitos no total, sendo 2 após a vírgula

            base.OnModelCreating(modelBuilder);
        }
    }
}