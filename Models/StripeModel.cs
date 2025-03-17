namespace DigitalStore.Models
{
    // Modelo para armazenar as informações necessárias para uma transação com Stripe
    public class StripeModel
    {
        // ID do usuário associado à transação (referencia o usuário que está realizando a compra)
        public int UsuarioId { get; set; }

        // ID do endereço do usuário associado à transação (referencia o endereço de entrega ou cobrança)
        public string EnderecoCompleto { get; set; }

        // Token do cartão de crédito gerado no frontend, utilizado para processar a transação no Stripe
        public required string Token { get; set; }

        // Valor total da compra (o preço total que será cobrado)
        public decimal Valor { get; set; }

        // Moeda utilizada na transação, com valor padrão definido como "brl" (real brasileiro)
        public string Moeda { get; set; } = "brl";

        // Descrição do produto ou serviço que está sendo comprado (detalhes do que está sendo pago)
        public string? Descricao { get; set; }
    }
}
