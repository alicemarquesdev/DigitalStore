namespace DigitalStore.Helper
{
    // Classe usada para armazenar as configurações do Stripe (SecretKey e PublishableKey).
    // A classe é configurada no Program.cs, onde as configurações do Stripe são lidas do appsettings.json 
    // e injetadas automaticamente através da injeção de dependência.
    // As chaves serão carregadas a partir do appsettings.json, onde estarão armazenadas de maneira segura.
    public class StripeSettings
    {
        // A chave secreta do Stripe, que será utilizada para autenticar a aplicação com a API do Stripe.
        public string SecretKey { get; set; }

        // A chave pública do Stripe, que pode ser usada para a integração do frontend.
        public string PublishableKey { get; set; }
    }
}
