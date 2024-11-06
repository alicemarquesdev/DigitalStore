using DigitalStore.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace DigitalStore.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Smartphones()
        {
            return View();
        }

        public IActionResult Notebooks()
        {
            return View();
        }

        public IActionResult Headphones()
        {
            return View();
        }

        public IActionResult Relogios()
        {
            return View();
        }

        public IActionResult Acessorios()
        {

            return View();
        }

        public IActionResult Favoritos()
        {
            return View();
        }
        public IActionResult Carrinho()
        {
            return View();
        }

        public IActionResult Suporte()
        {
            return View();
        }



        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
