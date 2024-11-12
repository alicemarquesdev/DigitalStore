using DigitalStore.Models;
using DigitalStore.Repositorio;
using Microsoft.AspNetCore.Mvc;

namespace DigitalStore.Controllers
{
    public class FavoritoController : Controller
    {
        private readonly IFavoritosRepositorio _favoritosRepositorio;

        public FavoritoController(IFavoritosRepositorio favoritosRepositorio)
        {
            _favoritosRepositorio = favoritosRepositorio;
        }

        [HttpPost]
        public async Task<IActionResult> AddFavorito(int produtoId, int usuarioId)
        {
            await _favoritosRepositorio.AddFavoritoAsync(produtoId, usuarioId);
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Index(int usuarioId)
        {
            List<FavoritosModel> favoritos = await _favoritosRepositorio.BuscarFavoritosDoUsuarioAsync(usuarioId);
            return View(favoritos);
        }

        [HttpPost]
        public async Task<IActionResult> RemoverFavorito(int produtoId, int usuarioId)
        {
            await _favoritosRepositorio.RemoverFavoritoAsync(produtoId, usuarioId);
            return RedirectToAction("Index");
        }
    }
}