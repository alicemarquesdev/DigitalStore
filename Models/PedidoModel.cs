using DigitalStore.Enums;
using System.ComponentModel.DataAnnotations;

namespace DigitalStore.Models
{
    // Representa um pedido realizado por um usuário
    public class PedidoModel
    {
        // ID único para o pedido
        [Key]
        public int PedidoId { get; set; }

        // Endereco associado ao pedido
        public required string Endereco { get; set; }

        // Data e hora em que o pedido foi realizado. O valor padrão é o momento atual da criação.
        public DateTime DataDoPedido { get; set; } = DateTime.Now;

        // Status atual do pedido (ex.: Pago, Em Processamento, Enviado, etc.)
        // O valor padrão é 'Pago'
        public PedidoEnum StatusDoPedido { get; set; } = PedidoEnum.Pendente;

        // ID do usuário que fez o pedido. Relacionamento com UsuarioModel
        public int UsuarioId { get; set; }

        // Relacionamento com a entidade de Usuário (um pedido pertence a um único usuário)
        public virtual UsuarioModel? Usuario { get; set; }

        // ID do pagamento associado ao pedido. Relacionamento com PagamentoModel
        public int PagamentoId { get; set; }

        // Relacionamento com a entidade Pagamento (um pedido pode ter um pagamento associado)
        public virtual PagamentoModel? Pagamento { get; set; }

        // Lista de itens do pedido, representando os produtos incluídos no pedido
        public virtual IList<ItensDoPedidoModel> ItensDoPedido { get; set; } = new List<ItensDoPedidoModel>();
    }
}
