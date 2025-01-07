using System.ComponentModel.DataAnnotations;

namespace DigitalStore.Models
{
    public class ItensDoPedidoModel
    {
        [Key]
        public int ItemId { get; set; }

        public int PedidoId { get; set; }

        public int ProdutoId { get; set; }

        public int QuantidadeDeProdutos { get; set; }

        public decimal PrecoUnidadeProduto { get; set; }

        public virtual PedidoModel Pedido { get; set; } = new PedidoModel();// Relacionamento com PedidosModel

        public virtual ProdutoModel Produto { get; set; } = new ProdutoModel(); // Relacionamento com ProdutoModel (model que você precisaria criar para representar os produtos)
    }
}