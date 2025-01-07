using DigitalStore.Enums;
using System.ComponentModel.DataAnnotations;

namespace DigitalStore.Models
{
    public class PedidoModel
    {
        [Key]
        public int PedidoId { get; set; }

        public DateTime DataDoPedido { get; set; } = DateTime.Now;

        public decimal ValorTotalDoPedido { get; set; }

        public PedidoEnum StatusDoPedido { get; set; } = PedidoEnum.Pendente;

        public PagamentoEnum StatusPagamento { get; set; } = PagamentoEnum.Pendente;

        public int? EnderecoId { get; set; }

        public virtual EnderecoModel Endereco { get; set; }
        public int UsuarioId { get; set; }

        public virtual UsuarioModel Usuario { get; set; }

        public int? PagamentoId { get; set; }

        public virtual PagamentoModel Pagamento { get; set; }

        public virtual IList<ItensDoPedidoModel> ItensDoPedido { get; set; }
    }
}