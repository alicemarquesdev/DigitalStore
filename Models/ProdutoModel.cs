using DigitalStore.Enums;
using System.ComponentModel.DataAnnotations;

namespace DigitalStore.Models
{
    // Modelo para representar um produto
    public class ProdutoModel
    {
        // ID único para o produto 
        [Key]
        public int ProdutoId { get; set; }

        // Nome do produto
        [Required(ErrorMessage = "Insira o nome do produto.")]
        [StringLength(30, ErrorMessage = "O nome do produto deve ter no máximo 30 caracteres.")]
        public required string NomeProduto { get; set; }

        // Descrição do produto
        [Required(ErrorMessage = "Insira a descrição do produto.")]
        [StringLength(100, ErrorMessage = "O nome do produto deve ter no máximo 100 caracteres.")]
        public required string Descricao { get; set; }

        // Categoria do produto (ex.: Eletrônicos, Roupas, etc.)
        [Required(ErrorMessage = "Insira a categoria do produto.")]
        [StringLength(30, ErrorMessage = "O nome do produto deve ter no máximo 30 caracteres.")]
        public required string Categoria { get; set; }

        // Preço do produto
        [Required(ErrorMessage = "Insira o preço do produto.")]
        public decimal Preco { get; set; }

        // Quantidade em estoque do produto
        [Required(ErrorMessage = "Insira a quantidade do produto em estoque.")]
        [Range(1, 1000, ErrorMessage = "A quantidade em estoque deve ser entre 1 e 1000.")]
        public int QuantidadeEstoque { get; set; } = 1;

        // URL da imagem do produto,a imagem é salva em wwwroot/image
        public string? ImagemUrl { get; set; }

        // Data de cadastro do produto
        // O valor padrão é a data e hora atual
        public DateTime DataCadastro { get; set; } = DateTime.Now;

        // Relacionamento com o Carrinho
        // Lista de carrinhos em que o produto foi adicionado
        public virtual IList<CarrinhoModel> Carrinho { get; set; } = new List<CarrinhoModel>();

        // Relacionamento com os Favoritos
        // Lista de favoritos em que o produto foi adicionado
        public virtual IList<FavoritosModel> Favoritos { get; set; } = new List<FavoritosModel>();

        // Relacionamento com Itens do Pedido
        // Lista de itens do pedido em que o produto foi incluído
        public virtual IList<ItensDoPedidoModel> ItensDoPedido { get; set; } = new List<ItensDoPedidoModel>();
    }
}
