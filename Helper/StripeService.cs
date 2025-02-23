using Stripe;

public class StripeService
{
    private readonly string _secretKey;

    public StripeService(IConfiguration configuration)
    {
        _secretKey = configuration["StripeSettings:SecretKey"] ?? throw new ArgumentNullException(nameof(configuration), "Stripe secret key cannot be null");
        StripeConfiguration.ApiKey = _secretKey;
    }

    public Charge CriarCobrança(string token, int valor, string descricao)
    {
        var options = new ChargeCreateOptions
        {
            Amount = valor * 100,
            Currency = "brl",
            Description = descricao,
            Source = token
        };

        var service = new ChargeService();
        return service.Create(options);
    }
}
