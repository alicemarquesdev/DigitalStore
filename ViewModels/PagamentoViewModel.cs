using DigitalStore.Models;

namespace DigitalStore.ViewModels
{
    // ViewModel para a View Pagamento
    public class PagamentoViewModel
    {
        // ID do usuário para associar a transação
        public int UsuarioId { get; set; }

        // Total de itens no carrinho antes da adição de frete
        public decimal CarrinhoTotal { get; set; }

        // Valor do frete calculado
        public decimal Frete { get; set; }

        // Valor total a ser pago (carrinho + frete)
        public decimal Valor { get; set; }

        // Passar a chave p[ublica do Stripe para o frontend
        public string? StripeKey { get; set; }

        // Endereço de entrega do usuário
        public EnderecoModel? Endereco { get; set; }
    }
}
