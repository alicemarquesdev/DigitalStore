using DigitalStore.Enums;
using System.ComponentModel.DataAnnotations;

namespace DigitalStore.Models
{
    // Representa um pagamento associado a um pedido.
    public class PagamentoModel
    {
        // ID único para o pagamento
        [Key]
        public int PagamentoId { get; set; }

        // ID do pedido ao qual o pagamento está vinculado
        public int PedidoId { get; set; }

        // Método de pagamento utilizado (ex.: Cartão, Boleto)
        public MetodoPagamentoEnum MetodoPagamento { get; set; }

        // Status atual do pagamento (ex.: Pendente, Pago..)
        public StatusPagamentoEnum StatusPagamento { get; set; }

        // Valor total do pagamento
        public decimal Valor { get; set; }

        // Token gerado pela API do Stripe para transações (usado quando o pagamento é realizado via Stripe)
        public string? TokenStripe { get; set; }

        // Data e hora em que o pagamento foi realizado
        // O valor default é o momento atual
        public DateTime DataDoPagamento { get; set; } = DateTime.Now;

        // Relacionamento com o modelo de Pedido (um pagamento pertence a um pedido)
        public virtual PedidoModel? Pedido { get; set; }
    }
}
