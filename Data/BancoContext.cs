using DigitalStore.Models;
using Microsoft.EntityFrameworkCore;

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
            modelBuilder.Entity<CarrinhoModel>()
             .HasKey(ec => new { ec.UsuarioId, ec.ProdutoId });

            modelBuilder.Entity<CarrinhoModel>()
                .HasOne(ec => ec.Usuario)
                .WithMany(e => e.Carrinho)
                .HasForeignKey(ec => ec.UsuarioId);

            modelBuilder.Entity<CarrinhoModel>()
                .HasOne(ec => ec.Produto)
                .WithMany(c => c.Carrinho)
                .HasForeignKey(ec => ec.ProdutoId);

            modelBuilder.Entity<FavoritosModel>()
             .HasKey(ec => new { ec.UsuarioId, ec.ProdutoId });

            modelBuilder.Entity<FavoritosModel>()
                .HasOne(ec => ec.Usuario)
                .WithMany(e => e.Favoritos)
                .HasForeignKey(ec => ec.UsuarioId);

            modelBuilder.Entity<FavoritosModel>()
                .HasOne(ec => ec.Produto)
                .WithMany(c => c.Favoritos)
                .HasForeignKey(ec => ec.ProdutoId);

            // Configurar o tipo de coluna da propriedade 'Preco' com precisão e escala
            modelBuilder.Entity<ProdutoModel>()
                .Property(p => p.Preco)
                .HasColumnType("decimal(18,2)");  // Exemplo: 18 dígitos no total, sendo 2 após a vírgula

            base.OnModelCreating(modelBuilder);
        }
    }
}