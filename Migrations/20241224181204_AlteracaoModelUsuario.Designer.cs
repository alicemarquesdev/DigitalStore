﻿// <auto-generated />
using System;
using DigitalStore.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace DigitalStore.Migrations
{
    [DbContext(typeof(BancoContext))]
    [Migration("20241224181204_AlteracaoModelUsuario")]
    partial class AlteracaoModelUsuario
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.8")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("DigitalStore.Models.CarrinhoModel", b =>
                {
                    b.Property<int>("ProdutoId")
                        .HasColumnType("int");

                    b.Property<int>("UsuarioId")
                        .HasColumnType("int");

                    b.HasKey("ProdutoId", "UsuarioId");

                    b.HasIndex("UsuarioId");

                    b.ToTable("Carrinho");
                });

            modelBuilder.Entity("DigitalStore.Models.EnderecoModel", b =>
                {
                    b.Property<int>("EnderecoId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("EnderecoId"));

                    b.Property<string>("Bairro")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("CEP")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)");

                    b.Property<string>("Cidade")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("Estado")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("Pais")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("Rua")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.HasKey("EnderecoId");

                    b.ToTable("Endereco");
                });

            modelBuilder.Entity("DigitalStore.Models.FavoritosModel", b =>
                {
                    b.Property<int>("ProdutoId")
                        .HasColumnType("int");

                    b.Property<int>("UsuarioId")
                        .HasColumnType("int");

                    b.HasKey("ProdutoId", "UsuarioId");

                    b.HasIndex("UsuarioId");

                    b.ToTable("Favoritos");
                });

            modelBuilder.Entity("DigitalStore.Models.ItensDoPedidoModel", b =>
                {
                    b.Property<int>("PedidoId")
                        .HasColumnType("int");

                    b.Property<int>("ProdutoId")
                        .HasColumnType("int");

                    b.Property<int>("ItemId")
                        .HasColumnType("int");

                    b.Property<decimal>("PrecoUnidadeProduto")
                        .HasPrecision(18, 2)
                        .HasColumnType("decimal(18,2)");

                    b.Property<int>("QuantidadeDeProdutos")
                        .HasColumnType("int");

                    b.HasKey("PedidoId", "ProdutoId");

                    b.HasIndex("ProdutoId");

                    b.ToTable("ItensDoPedido");
                });

            modelBuilder.Entity("DigitalStore.Models.PagamentoModel", b =>
                {
                    b.Property<int>("PagamentoId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("PagamentoId"));

                    b.Property<DateTime>("DataDoPagamento")
                        .HasColumnType("datetime2");

                    b.Property<int>("MetodoPagamento")
                        .HasColumnType("int");

                    b.Property<int>("PedidoId")
                        .HasColumnType("int");

                    b.Property<int>("StatusPagamento")
                        .HasColumnType("int");

                    b.HasKey("PagamentoId");

                    b.HasIndex("PedidoId")
                        .IsUnique();

                    b.ToTable("Pagamentos");
                });

            modelBuilder.Entity("DigitalStore.Models.PedidoModel", b =>
                {
                    b.Property<int>("PedidoId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("PedidoId"));

                    b.Property<DateTime>("DataDoPedido")
                        .HasColumnType("datetime2");

                    b.Property<int>("EnderecoId")
                        .HasColumnType("int");

                    b.Property<int?>("EnderecoModelEnderecoId")
                        .HasColumnType("int");

                    b.Property<int>("PagamentoId")
                        .HasColumnType("int");

                    b.Property<int>("StatusDoPedido")
                        .HasColumnType("int");

                    b.Property<int>("StatusPagamento")
                        .HasColumnType("int");

                    b.Property<int>("UsuarioId")
                        .HasColumnType("int");

                    b.Property<decimal>("ValorTotalDoPedido")
                        .HasPrecision(18, 2)
                        .HasColumnType("decimal(18,2)");

                    b.HasKey("PedidoId");

                    b.HasIndex("EnderecoId");

                    b.HasIndex("EnderecoModelEnderecoId");

                    b.HasIndex("UsuarioId");

                    b.ToTable("Pedidos");
                });

            modelBuilder.Entity("DigitalStore.Models.ProdutoModel", b =>
                {
                    b.Property<int>("ProdutoId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ProdutoId"));

                    b.Property<string>("Categoria")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("DataCadastro")
                        .HasColumnType("datetime2");

                    b.Property<string>("Descricao")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ImagemUrl")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("NomeProduto")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<decimal>("Preco")
                        .HasColumnType("decimal(18,2)");

                    b.Property<int>("QuantidadeEstoque")
                        .HasColumnType("int");

                    b.Property<int?>("Status")
                        .HasColumnType("int");

                    b.HasKey("ProdutoId");

                    b.ToTable("Produtos");
                });

            modelBuilder.Entity("DigitalStore.Models.SiteModel", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Banner1Url")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Banner2Url")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Banner3Url")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Banner4Url")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Frase")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("NomeSite")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)");

                    b.HasKey("Id");

                    b.ToTable("Site");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Banner1Url = "banner-1.jpg",
                            Banner2Url = "",
                            Banner3Url = "",
                            Banner4Url = "",
                            Frase = "Tudo que você procura em um só lugar",
                            NomeSite = "DigitalStore"
                        });
                });

            modelBuilder.Entity("DigitalStore.Models.UsuarioModel", b =>
                {
                    b.Property<int>("UsuarioId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("UsuarioId"));

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Nome")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Perfil")
                        .HasColumnType("int");

                    b.Property<string>("Senha")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("UsuarioId");

                    b.ToTable("Usuarios");
                });

            modelBuilder.Entity("DigitalStore.Models.CarrinhoModel", b =>
                {
                    b.HasOne("DigitalStore.Models.ProdutoModel", "Produto")
                        .WithMany("Carrinho")
                        .HasForeignKey("ProdutoId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("DigitalStore.Models.UsuarioModel", "Usuario")
                        .WithMany("Carrinho")
                        .HasForeignKey("UsuarioId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Produto");

                    b.Navigation("Usuario");
                });

            modelBuilder.Entity("DigitalStore.Models.FavoritosModel", b =>
                {
                    b.HasOne("DigitalStore.Models.ProdutoModel", "Produto")
                        .WithMany("Favoritos")
                        .HasForeignKey("ProdutoId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("DigitalStore.Models.UsuarioModel", "Usuario")
                        .WithMany("Favoritos")
                        .HasForeignKey("UsuarioId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Produto");

                    b.Navigation("Usuario");
                });

            modelBuilder.Entity("DigitalStore.Models.ItensDoPedidoModel", b =>
                {
                    b.HasOne("DigitalStore.Models.PedidoModel", "Pedido")
                        .WithMany("ItensDoPedido")
                        .HasForeignKey("PedidoId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("DigitalStore.Models.ProdutoModel", "Produto")
                        .WithMany("ItensDoPedido")
                        .HasForeignKey("ProdutoId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Pedido");

                    b.Navigation("Produto");
                });

            modelBuilder.Entity("DigitalStore.Models.PagamentoModel", b =>
                {
                    b.HasOne("DigitalStore.Models.PedidoModel", "Pedido")
                        .WithOne("Pagamento")
                        .HasForeignKey("DigitalStore.Models.PagamentoModel", "PedidoId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Pedido");
                });

            modelBuilder.Entity("DigitalStore.Models.PedidoModel", b =>
                {
                    b.HasOne("DigitalStore.Models.EnderecoModel", "Endereco")
                        .WithMany()
                        .HasForeignKey("EnderecoId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("DigitalStore.Models.EnderecoModel", null)
                        .WithMany("Pedido")
                        .HasForeignKey("EnderecoModelEnderecoId");

                    b.HasOne("DigitalStore.Models.UsuarioModel", "Usuario")
                        .WithMany("Pedido")
                        .HasForeignKey("UsuarioId")
                        .IsRequired();

                    b.Navigation("Endereco");

                    b.Navigation("Usuario");
                });

            modelBuilder.Entity("DigitalStore.Models.EnderecoModel", b =>
                {
                    b.Navigation("Pedido");
                });

            modelBuilder.Entity("DigitalStore.Models.PedidoModel", b =>
                {
                    b.Navigation("ItensDoPedido");

                    b.Navigation("Pagamento")
                        .IsRequired();
                });

            modelBuilder.Entity("DigitalStore.Models.ProdutoModel", b =>
                {
                    b.Navigation("Carrinho");

                    b.Navigation("Favoritos");

                    b.Navigation("ItensDoPedido");
                });

            modelBuilder.Entity("DigitalStore.Models.UsuarioModel", b =>
                {
                    b.Navigation("Carrinho");

                    b.Navigation("Favoritos");

                    b.Navigation("Pedido");
                });
#pragma warning restore 612, 618
        }
    }
}
