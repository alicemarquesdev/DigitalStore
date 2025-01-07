using DigitalStore.Models;
using DigitalStore.Repositorio.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace DigitalStore.Controllers
{
    public class FavoritoController : Controller
    {
        // Declaração das dependências dos repositórios
        private readonly IFavoritosRepositorio _favoritosRepositorio;

        private readonly IUsuarioRepositorio _usuarioRepositorio;
        private readonly IProdutoRepositorio _produtoRepositorio;

        // Construtor para injeção de dependências
        public FavoritoController(IFavoritosRepositorio favoritosRepositorio,
                                    IUsuarioRepositorio usuarioRepositorio,
                                    IProdutoRepositorio produtoRepositorio)
        {
            _favoritosRepositorio = favoritosRepositorio;
            _usuarioRepositorio = usuarioRepositorio;
            _produtoRepositorio = produtoRepositorio;
        }

        public async Task<IActionResult> Favoritos(int id)
        {
            List<FavoritosModel> favoritos = await _favoritosRepositorio.BuscarFavoritosDoUsuarioAsync(id);
            return View(favoritos);
        }

        // Método para adicionar um produto aos favoritos
        [HttpPost]
        public async Task<IActionResult> AddFavorito(int produtoId, int usuarioId)
        {
            try
            {
                var urlAnterior = Request.Headers["Referer"].ToString();

                // Chama o repositório para adicionar o produto aos favoritos
                await _favoritosRepositorio.AddFavoritoAsync(produtoId, usuarioId);

                // Redireciona para a página de favoritos do cliente
                return Redirect(urlAnterior);
            }
            catch (Exception ex)
            {
                // Em caso de erro, loga a exceção e retorna um erro genérico
                Console.WriteLine(ex.Message);
                return StatusCode(500, "Erro ao adicionar produto aos favoritos.");
            }
        }

        // Método para remover um produto dos favoritos
        [HttpPost]
        public async Task<IActionResult> RemoverFavorito(int produtoId, int usuarioId)
        {
            try
            {
                var urlAnterior = Request.Headers["Referer"].ToString();

                // Chama o repositório para remover o produto dos favoritos
                await _favoritosRepositorio.RemoverFavoritoAsync(produtoId, usuarioId);

                // Redireciona para a página do carrinho do cliente
                return RedirectToAction("Carrinho", "Carrinho");
            }
            catch (Exception ex)
            {
                // Em caso de erro, loga a exceção e retorna um erro genérico
                Console.WriteLine(ex.Message);
                return StatusCode(500, "Erro ao remover produto dos favoritos.");
            }
        }

        // Método para alternar entre adicionar ou remover produto dos favoritos
        [HttpPost]
        public async Task<IActionResult> BotaoFavorito(int produtoId, int usuarioId)
        {
            try
            {
                var urlAnterior = Request.Headers["Referer"].ToString();

                // Busca o usuário e o produto
                var usuario = await _usuarioRepositorio.BuscarUsuarioPorIdAsync(usuarioId);
                var produto = await _produtoRepositorio.BuscarProdutoPorIdAsync(produtoId);

                // Verifica se o usuário e o produto existem
                if (usuario == null || produto == null)
                {
                    // Se o produto ou usuário não existir, retorna erro 404
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

                // Após a operação, redireciona para a página anterior
                return Redirect(urlAnterior);
            }
            catch (Exception ex)
            {
                // Em caso de erro, loga a exceção e retorna um erro genérico
                Console.WriteLine(ex.Message);
                return StatusCode(500, "Erro ao processar operação nos favoritos.");
            }
        }
    }
}