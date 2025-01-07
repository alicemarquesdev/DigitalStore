using DigitalStore.Models;
using Microsoft.EntityFrameworkCore;

namespace DigitalStore.Data
{
    public class BancoContext : DbContext
    {
        public BancoContext(DbContextOptions<BancoContext> options) : base(options)
        {
        }

        // DbSets para as entidades do banco de dados
        public DbSet<CarrinhoModel> Carrinho { get; set; }

        public DbSet<FavoritosModel> Favoritos { get; set; }
        public DbSet<ProdutoModel> Produtos { get; set; }
        public DbSet<UsuarioModel> Usuarios { get; set; }
        public DbSet<SiteModel> Site { get; set; }
        public DbSet<PedidoModel> Pedidos { get; set; }
        public DbSet<ItensDoPedidoModel> ItensDoPedido { get; set; }
        public DbSet<PagamentoModel> Pagamentos { get; set; }
        public DbSet<EnderecoModel> Endereco { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<SiteModel>().HasData(

                new SiteModel { Id = 1, NomeSite = "DigitalStore", Banner1Url = "banner-1.jpg", Frase = "Tudo que você procura em um só lugar" }
            );
            modelBuilder.Entity<EnderecoModel>()
               .HasOne(ac => ac.Usuario)
               .WithMany(c => c.Endereco)
               .HasForeignKey(ac => ac.UsuarioId)
               .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<PedidoModel>()
              .HasOne(p => p.Usuario)
              .WithMany(u => u.Pedido)
              .HasForeignKey(p => p.UsuarioId)
              .OnDelete(DeleteBehavior.ClientSetNull);

            modelBuilder.Entity<PedidoModel>()
                .HasOne(p => p.Endereco)
                .WithMany()
                .HasForeignKey(p => p.EnderecoId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<PedidoModel>()
              .HasOne(ac => ac.Pagamento)
              .WithOne(c => c.Pedido)
              .HasForeignKey<PedidoModel>(ac => ac.PagamentoId);

            modelBuilder.Entity<PedidoModel>()
               .Property(p => p.ValorTotalDoPedido)
               .HasPrecision(18, 2);

            modelBuilder.Entity<ItensDoPedidoModel>()
               .Property(p => p.PrecoUnidadeProduto)
               .HasPrecision(18, 2); // Exemplo: precisão de 18 dígitos, 2 casas decimais

            modelBuilder.Entity<ItensDoPedidoModel>()
                .HasKey(ac => new { ac.PedidoId, ac.ProdutoId });

            modelBuilder.Entity<ItensDoPedidoModel>()
                .HasOne(ac => ac.Pedido)
                .WithMany(c => c.ItensDoPedido)
                .HasForeignKey(ac => ac.PedidoId);

            modelBuilder.Entity<ItensDoPedidoModel>()
               .HasOne(ac => ac.Produto)
               .WithMany(c => c.ItensDoPedido)
               .HasForeignKey(ac => ac.ProdutoId);

            modelBuilder.Entity<PagamentoModel>()
              .HasOne(ac => ac.Pedido)
              .WithOne(c => c.Pagamento)
              .HasForeignKey<PagamentoModel>(ac => ac.PedidoId);

            // Configuração da chave composta para a tabela Favoritos
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

            // Configuração da chave composta para a tabela Carrinho
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

            // Configuração do tipo de coluna da propriedade 'Preco' da tabela Produto
            modelBuilder.Entity<ProdutoModel>()
                .Property(p => p.Preco)
                .HasColumnType("decimal(18,2)");  // Exemplo: 18 dígitos no total, sendo 2 após a vírgula

            base.OnModelCreating(modelBuilder);
        }
    }
}