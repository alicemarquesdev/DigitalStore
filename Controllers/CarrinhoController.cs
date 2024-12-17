using DigitalStore.Repositorio.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace DigitalStore.Controllers
{
    public class CarrinhoController : Controller
    {
        private readonly ICarrinhoRepositorio _carrinhoRepositorio;

        public CarrinhoController(ICarrinhoRepositorio carrinhoRepositorio)
        {
            _carrinhoRepositorio = carrinhoRepositorio;
        }

        [HttpPost]
        public async Task<IActionResult> AddCarrinho(int produtoId, int usuarioId)
        {
            await _carrinhoRepositorio.AddAoCarrinhoAsync(produtoId, usuarioId);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> RemoverCarrinho(int produtoId, int usuarioId)
        {
            await _carrinhoRepositorio.RemoverDoCarrinhoAsync(produtoId, usuarioId);
            return RedirectToAction("Index", "Carrinho");
        }
    }
}