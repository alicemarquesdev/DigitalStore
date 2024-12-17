using DigitalStore.Helper;
using DigitalStore.Models;
using DigitalStore.Repositorio.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace DigitalStore.Controllers
{
    public class PerfilClienteController : Controller
    {
        private readonly IProdutoRepositorio _produtoRepositorio;
        private readonly ISessao _sessao;
        private readonly IUsuarioRepositorio _usuarioRepositorio;
        private readonly ICarrinhoRepositorio _carrinhoRepositorio;
        private readonly IFavoritosRepositorio _favoritosRepositorio;

        public PerfilClienteController(IProdutoRepositorio produtoRepositorio,
                              IUsuarioRepositorio usuarioRepositorio,
                              ICarrinhoRepositorio carrinhoRepositorio,
                              IFavoritosRepositorio favoritosRepositorio,
                              ISessao sessao)
        {
            _produtoRepositorio = produtoRepositorio;
            _usuarioRepositorio = usuarioRepositorio;
            _carrinhoRepositorio = carrinhoRepositorio;
            _favoritosRepositorio = favoritosRepositorio;
            _sessao = sessao;
        }

        public IActionResult ConfirmarPagamento()
        {
           
            return View();
        }

        public IActionResult MeusPedidos()
        {

            return View();
        }

        public async Task<IActionResult> Carrinho(int id)
        {

            List<CarrinhoModel> produtos = await _carrinhoRepositorio.BuscarCarrinhoDoUsuarioAsync(id);
            return View(produtos);
        }

        public async Task<IActionResult> Favoritos(int id)
        {
            List<FavoritosModel> favoritos = await _favoritosRepositorio.BuscarFavoritosDoUsuarioAsync(id);
            return View(favoritos);
        }
    }
}