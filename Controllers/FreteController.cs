using DigitalStore.Helper;
using DigitalStore.Repositorio.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace DigitalStore.Controllers
{
    public class FreteController : Controller
    {
        private readonly FreteServicos _freteServicos;
        private readonly IEnderecoRepositorio _enderecoRepositorio;

        public FreteController(FreteServicos freteServicos,
                                IEnderecoRepositorio enderecoRepositorio)
        {
            _freteServicos = freteServicos;
            _enderecoRepositorio = enderecoRepositorio;
        }

        [HttpPost]
        public async Task<IActionResult> CalcularFrete(string cep)
        {
            var urlAnterior = Request.Headers["Referer"].ToString();

            if (string.IsNullOrEmpty(cep) || cep.Length != 8)
            {
                TempData["MensagemErro"] = "CEP inválido. Insira um CEP com 8 dígitos.";
                return Redirect(urlAnterior);
            }
            decimal valorFrete = await _freteServicos.CalcularFreteComAPIAsync(cep);
            ViewBag.ValorFrete = valorFrete;
            ViewBag.CEP = cep;

            return RedirectToAction("Pagamento", "Pagamento");
        }
    }
}