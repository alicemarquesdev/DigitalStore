using DigitalStore.Repositorio;
using Microsoft.AspNetCore.Mvc;

namespace DigitalStore.Controllers
{
    public class FavoritoController : Controller
    {
        private readonly IProdutoRepositorio _produtoRepositorio;

        public FavoritoController(IProdutoRepositorio produtoRepositorio)
        {
            _produtoRepositorio = produtoRepositorio;
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}