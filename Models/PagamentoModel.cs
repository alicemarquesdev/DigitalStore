using DigitalStore.Enums;
using System.ComponentModel.DataAnnotations;

namespace DigitalStore.Models
{
    public class PagamentoModel
    {
        [Key]
        public int PagamentoId { get; set; }

        public int PedidoId { get; set; }

        public MetodoPagamentoEnum MetodoPagamento { get; set; }

        public PagamentoEnum StatusPagamento { get; set; }

        public DateTime DataDoPagamento { get; set; } = DateTime.Now;  // Valor default da data

        public virtual PedidoModel Pedido { get; set; }  // Relacionamento com PedidosModel
    }
}