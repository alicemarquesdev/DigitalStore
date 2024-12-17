using DigitalStore.Models;
using Microsoft.EntityFrameworkCore;

namespace DigitalStore.Data
{
    public class BancoContext : DbContext
    {
        public BancoContext(DbContextOptions<BancoContext> options) : base(options)
        {
        }

        public DbSet<CarrinhoModel> Carrinho { get; set; }
        public DbSet<FavoritosModel> Favoritos { get; set; }
        public DbSet<ProdutoModel> Produtos { get; set; }
        public DbSet<UsuarioModel> Usuarios { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<FavoritosModel>()
              .HasKey(ac => new { ac.ProdutoId, ac.UsuarioId });

            modelBuilder.Entity<FavoritosModel>()
                .HasOne(ac => ac.Produto)
                .WithMany(a => a.Favoritos)
                .HasForeignKey(ac => ac.ProdutoId);

            modelBuilder.Entity<FavoritosModel>()
                .HasOne(ac => ac.Usuario)
                .WithMany(c => c.Favoritos)
                .HasForeignKey(ac => ac.UsuarioId);

            modelBuilder.Entity<CarrinhoModel>()
             .HasKey(ac => new { ac.ProdutoId, ac.UsuarioId });

            modelBuilder.Entity<CarrinhoModel>()
                .HasOne(ac => ac.Produto)
                .WithMany(a => a.Carrinho)
                .HasForeignKey(ac => ac.ProdutoId);

            modelBuilder.Entity<CarrinhoModel>()
                .HasOne(ac => ac.Usuario)
                .WithMany(c => c.Carrinho)
                .HasForeignKey(ac => ac.UsuarioId);

            // Configurar o tipo de coluna da propriedade 'Preco' com precisão e escala
            modelBuilder.Entity<ProdutoModel>()
                .Property(p => p.Preco)
                .HasColumnType("decimal(18,2)");  // Exemplo: 18 dígitos no total, sendo 2 após a vírgula

            base.OnModelCreating(modelBuilder);
        }
    }
}