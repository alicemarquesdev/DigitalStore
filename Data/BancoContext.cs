using DigitalStore.Models;
using Microsoft.EntityFrameworkCore;

namespace DigitalStore.Data
{
    // Classe responsável por representar o contexto do banco de dados da aplicação.
    // Utiliza o Entity Framework Core para mapear as entidades do sistema e configurar
    // as relações entre elas no banco de dados relacional (SQL Server).
    // Aqui são definidos os DbSets (tabelas) e as regras de relacionamento entre os modelos.

    public class BancoContext : DbContext
    {
        public BancoContext(DbContextOptions<BancoContext> options) : base(options)
        {
        }

        // Definição das tabelas do banco de dados
        public DbSet<SiteModel> Site { get; set; }
        public DbSet<UsuarioModel> Usuarios { get; set; }
        public DbSet<ProdutoModel> Produtos { get; set; }
        public DbSet<CarrinhoModel> Carrinho { get; set; }
        public DbSet<FavoritosModel> Favoritos { get; set; }
        public DbSet<PedidoModel> Pedidos { get; set; }
        public DbSet<ItensDoPedidoModel> ItensDoPedido { get; set; }
        public DbSet<PagamentoModel> Pagamentos { get; set; }
        public DbSet<EnderecoModel> Enderecos { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configuração das relações entre as tabelas no banco de dados relacional (SQL Server).
            // Definição de chaves primárias, estrangeiras e comportamentos de exclusão.
            // O Entity Framework mapeia essas relações para criar as constraints corretas no banco.

            // Seed (dados iniciais) para a tabela SiteModel
            modelBuilder.Entity<SiteModel>().HasData(
                new SiteModel { Id = 1, NomeSite = "DigitalStore", Banner = "~/image/banner.jpg", Frase = "Tudo que você procura em um só lugar" }
            );

            // Relacionamento 1:N entre Usuário e Endereço (um usuário pode ter vários endereços)
            modelBuilder.Entity<EnderecoModel>()
               .HasOne(ac => ac.Usuario)
               .WithMany(c => c.Endereco)
               .HasForeignKey(ac => ac.UsuarioId)
               .OnDelete(DeleteBehavior.Cascade);

            // Relacionamento 1:N entre Usuário e Pedido (um usuário pode ter vários pedidos)
            modelBuilder.Entity<PedidoModel>()
              .HasOne(p => p.Usuario)
              .WithMany(u => u.Pedido)
              .HasForeignKey(p => p.UsuarioId)
              .OnDelete(DeleteBehavior.ClientSetNull);

            // Relacionamento 1:1 entre Pedido e Pagamento (um pedido tem um único pagamento)
            modelBuilder.Entity<PedidoModel>()
              .HasOne(ac => ac.Pagamento)
              .WithOne(c => c.Pedido)
              .HasForeignKey<PedidoModel>(ac => ac.PagamentoId);

            // Relacionamento 1:N entre ItensDoPedido e Pedido
            modelBuilder.Entity<ItensDoPedidoModel>()
                .HasOne(ac => ac.Pedido)
                .WithMany(c => c.ItensDoPedido)
                .HasForeignKey(ac => ac.PedidoId);

            // Relacionamento 1:N entre ItensDoPedido e Produto
            modelBuilder.Entity<ItensDoPedidoModel>()
               .HasOne(ac => ac.Produto)
               .WithMany(c => c.ItensDoPedido)
               .HasForeignKey(ac => ac.ProdutoId);

            // Chave composta para ItensDoPedido (PedidoId + ProdutoId)
            modelBuilder.Entity<ItensDoPedidoModel>()
                .HasKey(ac => new { ac.PedidoId, ac.ProdutoId });

            // Configuração de precisão para o preço do item no pedido
            modelBuilder.Entity<ItensDoPedidoModel>()
               .Property(p => p.PrecoUnidadeItem)
               .HasPrecision(10, 2); // Define até 10 dígitos no total, sendo 2 casas decimais

            // Relacionamento 1:N entre Favoritos e Produto
            modelBuilder.Entity<FavoritosModel>()
                .HasOne(ac => ac.Produto)
                .WithMany(a => a.Favoritos)
                .HasForeignKey(ac => ac.ProdutoId);

            // Relacionamento 1:N entre Favoritos e Usuário
            modelBuilder.Entity<FavoritosModel>()
                .HasOne(ac => ac.Usuario)
                .WithMany(c => c.Favoritos)
                .HasForeignKey(ac => ac.UsuarioId);

            // Chave composta para a tabela Favoritos (ProdutoId + UsuarioId)
            modelBuilder.Entity<FavoritosModel>()
                .HasKey(ac => new { ac.ProdutoId, ac.UsuarioId });

            // Relacionamento 1:N entre Carrinho e Produto
            modelBuilder.Entity<CarrinhoModel>()
                .HasOne(ac => ac.Produto)
                .WithMany(a => a.Carrinho)
                .HasForeignKey(ac => ac.ProdutoId);

            // Relacionamento 1:N entre Carrinho e Usuário
            modelBuilder.Entity<CarrinhoModel>()
                .HasOne(ac => ac.Usuario)
                .WithMany(c => c.Carrinho)
                .HasForeignKey(ac => ac.UsuarioId);

            // Chave composta para a tabela Carrinho (ProdutoId + UsuarioId)
            modelBuilder.Entity<CarrinhoModel>()
                .HasKey(ac => new { ac.ProdutoId, ac.UsuarioId });

            // Configuração do tipo de coluna da propriedade 'Preco' da tabela Produto
            modelBuilder.Entity<ProdutoModel>()
                .Property(p => p.Preco)
                .HasColumnType("decimal(10,2)");  // Define até 10 dígitos no total, sendo 2 casas decimais

            // Relacionamento 1:1 entre Pagamento e Pedido
            modelBuilder.Entity<PagamentoModel>()
              .HasOne(ac => ac.Pedido)
              .WithOne(c => c.Pagamento)
              .HasForeignKey<PagamentoModel>(ac => ac.PedidoId);

            // Valor decimal pode ter até 8 dígitos inteiros e 2 decimais
            modelBuilder.Entity<PagamentoModel>()
               .Property(p => p.Valor)
               .HasColumnType("decimal(10,2)");

            base.OnModelCreating(modelBuilder);
        }
    }
}
