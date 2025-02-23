namespace DigitalStore.Models.ViewModels
{
    public class PagamentoViewModel
    {
        public int UsuarioId { get; set; }
        public EnderecoModel Endereco { get; set; }

        public decimal CarrinhoTotal { get; set; }

        public string StripeKey { get; set; }

        public decimal Frete { get; set; }
        public PagamentoModel Pagamento { get; set; } = new PagamentoModel();

        public StripeModel Stripe { get; set; } = new StripeModel();

        public decimal Valor { get; set; }
        public string Moeda { get; set; }
        public string Descricao { get; set; }
        public string Token { get; set; }
    }
}