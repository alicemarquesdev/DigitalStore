using Microsoft.AspNetCore.Mvc;

namespace DigitalStore.Controllers
{
    public class RestritoController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}