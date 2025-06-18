using Microsoft.AspNetCore.Mvc;

namespace DigitalStore.Controllers
{
    public class FavoritosController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
