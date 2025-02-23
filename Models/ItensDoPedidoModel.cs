using System.ComponentModel.DataAnnotations;

namespace DigitalStore.Models
{
    // Representa os itens de um pedido, incluindo o produto, a quantidade e o preço.
    public class ItensDoPedidoModel
    {
        // ID único do item no pedido.
        [Key]
        public int ItemId { get; set; }

        // ID do pedido ao qual o item pertence.
        public int PedidoId { get; set; }

        // ID do produto incluído no pedido.
        public int ProdutoId { get; set; }

        // Quantidade do produto no item do pedido.
        public int QuantidadeItem { get; set; }

        // Preço unitário do produto no momento da compra.
        public decimal PrecoUnidadeItem { get; set; }

        // Objeto que acessa os detalhes do pedido ao qual o item pertence.
        public virtual PedidoModel? Pedido { get; set; }

        // Objeto que acessa os detalhes do produto incluído no pedido.
        public virtual ProdutoModel? Produto { get; set; }
    }
}
