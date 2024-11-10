using DigitalStore.Enums;
using System.ComponentModel.DataAnnotations;

namespace DigitalStore.Models
{
    public class ProdutoModel
    {
        [Key]
        public int ProdutoId { get; set; }

        [Required(ErrorMessage = "Insira o nome do produto.")]
        public string NomeProduto { get; set; } = string.Empty;

        [Required(ErrorMessage = "Insira a descrição do produto.")]
        public string Descricao { get; set; } = string.Empty;

        [Required(ErrorMessage = "Insira a quantidade do produto em estoque.")]
        public int QuantidadeEstoque { get; set; }

        [Required(ErrorMessage = "Insira o preço do produto.")]
        public decimal Preco { get; set; } = 0.00m;

        [Required(ErrorMessage = "Insira a categoria do produto.")]
        public string Categoria { get; set; } = string.Empty;

        [Required(ErrorMessage = "Insira uma imagem do produto.")]
        public string ImagemUrl { get; set; } = string.Empty;

        public DateTime DataCadastro { get; set; } = DateTime.Now;

        public StatusEnum Status { get; set; } = StatusEnum.Ativo;

        public virtual List<FavoritosModel> Favoritos { get; set; } = new List<FavoritosModel>();

        public virtual List<CarrinhoModel> Carrinho { get; set; } = new List<CarrinhoModel>();
    }
}