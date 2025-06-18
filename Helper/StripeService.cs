using DigitalStore.Helper;
using Microsoft.Extensions.Options;
using Stripe;

// Classe usada para fazer o pagamento usando a API do Stripe
public class StripeService
{
    // A chave secreta usada para autenticar a aplicação com a API do Stripe.
    private readonly string _secretKey;


    // A chave é carregada da configuração que foi registrada usando o StripeSettings.
    public StripeService(IOptions<StripeSettings> stripeSettings)
    {
        // Obtém a chave secreta a partir da instância do StripeSettings que foi configurada
        _secretKey = stripeSettings.Value.SecretKey ?? throw new ArgumentNullException(nameof(stripeSettings), "Stripe secret key cannot be null");

        // Define a chave secreta para a biblioteca Stripe.
        // Configurada para ser utilizada automaticamente nas requisições para a API do Stripe
        StripeConfiguration.ApiKey = _secretKey;
    }

    // Método para criar uma cobrança com a API do Stripe.
    // Recebe o token do cartão, valor da cobrança e uma descrição da cobrança.
    public Charge CriarCobranca(string token, int valor, string? descricao)
    {
        // Cria as opções da cobrança, incluindo o valor, moeda, descrição e o token do cartão de crédito.
        var options = new ChargeCreateOptions
        {
            Amount = valor, // O Stripe espera o valor em centavos, então multiplicamos por 100.
            Currency = "brl", // Define a moeda como BRL (real brasileiro).
            Description = descricao, // Descrição da cobrança.
            Source = token // Token do cartão de crédito gerado pelo frontend (seguro).
        };

        // Cria uma instância do ChargeService, que é responsável por interagir com a API do Stripe.
        var service = new ChargeService();

        // Cria a cobrança na API do Stripe e retorna o resultado (objeto Charge).
        return service.Create(options);
    }
}
