using DigitalStore.Repositorio.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DigitalStore.Controllers
{
    public class FavoritoController : Controller
    {
        private readonly IFavoritosRepositorio _favoritosRepositorio;
        private readonly IUsuarioRepositorio _usuarioRepositorio;
        private readonly IProdutoRepositorio _produtoRepositorio;

        public FavoritoController(IFavoritosRepositorio favoritosRepositorio,
                                    IUsuarioRepositorio usuarioRepositorio,
                                    IProdutoRepositorio produtoRepositorio)
        {
            _favoritosRepositorio = favoritosRepositorio;
            _usuarioRepositorio = usuarioRepositorio;
            _produtoRepositorio = produtoRepositorio;
        }

        [HttpPost]
        public async Task<IActionResult> AddFavorito(int produtoId, int usuarioId)
        {
            try
            {
                await _favoritosRepositorio.AddFavoritoAsync(produtoId, usuarioId);
                return RedirectToAction("Favoritos", "PerfilCliente");
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> RemoverFavorito(int produtoId, int usuarioId)
        {
            try
            {
                await _favoritosRepositorio.RemoverFavoritoAsync(produtoId, usuarioId);
                return RedirectToAction("Carrinho", "PerfilCliente");
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> BotaoFavorito(int produtoId, int usuarioId)
        {
            // Busca o usuário e o produto
            var usuario = await _usuarioRepositorio.BuscarUsuarioPorIdAsync(usuarioId);
            var produto = await _produtoRepositorio.BuscarProdutoPorIdAsync(produtoId);

            // Verifica se o usuário e o produto existem
            if (usuario == null || produto == null)
            {
                // Se o produto ou usuário não existir, redireciona para uma página de erro ou retorna algo apropriado
                return NotFound(); // Ou redireciona para uma página de erro
            }

            // Verifica se o produto já está nos favoritos
            var favoritoExistente = await _favoritosRepositorio.BuscarProdutoExistenteNoFavoritosAsync(produtoId, usuarioId);

            if (favoritoExistente != null)
            {
                // Remove o produto dos favoritos
                await RemoverFavorito(produtoId, usuarioId);
            }
            else
            {
                // Adiciona o produto aos favoritos
                await AddFavorito(produtoId, usuarioId);
            }

            // Após a operação, redireciona para a categoria do produto
            return RedirectToAction("Categoria", "Home", new { categoria = produto.Categoria });
        }


    }
}