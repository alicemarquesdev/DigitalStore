using DigitalStore.Helper;
using DigitalStore.Models;
using DigitalStore.Repositorio;
using DigitalStore.Repositorio.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Stripe;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;
using Stripe.Checkout;

namespace DigitalStore.Controllers
{
    public class PagamentoController : Controller
    {
        private readonly StripeSettings _stripeSettings;

        public PagamentoController(IOptions<StripeSettings> stripeSettings)
        {
            _stripeSettings = stripeSettings.Value;
            StripeConfiguration.ApiKey = _stripeSettings.SecretKey; // Definir a chave secreta do Stripe
        }

        // Ação que carrega a página de pagamento
        public IActionResult Pagamento()
        {
            ViewData["PublishableKey"] = _stripeSettings.PublishableKey;
            return View();
        }

        // Ação que mostra a página de sucesso
        public IActionResult Sucesso()
        {
            return View();
        }

        // Ação que mostra a página de cancelamento
        public IActionResult Cancelamento()
        {
            return View();
        }

        // Ação para processar o pagamento via Stripe Checkout
        [HttpPost]
        public IActionResult ProcessarPagamento()
        {
            var options = new PaymentIntentCreateOptions
            {
                Amount = 5000, // Valor em centavos (R$ 50,00)
                Currency = "brl",
                PaymentMethodTypes = new List<string> { "card" },
            };

            var service = new PaymentIntentService();
            var paymentIntent = service.Create(options);

            // Retorna o clientSecret para o frontend
            return Json(new { clientSecret = paymentIntent.ClientSecret });
        }

    }
}
