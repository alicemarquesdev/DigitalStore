using DigitalStore.Helper;
using DigitalStore.Models;
using DigitalStore.Models.ViewModels;
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
        private readonly ISessao _sessao;
        private readonly ICarrinhoRepositorio _carrinhoRepositorio;

        // Construtor para injeção de dependências
        public FavoritoController(IFavoritosRepositorio favoritosRepositorio,
                                    IUsuarioRepositorio usuarioRepositorio,
                                    IProdutoRepositorio produtoRepositorio,
                                    ISessao sessao,
                                    ICarrinhoRepositorio carrinhoRepositorio)
        {
            _favoritosRepositorio = favoritosRepositorio;
            _usuarioRepositorio = usuarioRepositorio;
            _produtoRepositorio = produtoRepositorio;
            _sessao = sessao;
            _carrinhoRepositorio = carrinhoRepositorio;
        }

        public async Task<IActionResult> Favoritos(int id)
        {
            List<CarrinhoModel> carrinho = await _carrinhoRepositorio.BuscarCarrinhoDoUsuarioAsync(id);
            List<FavoritosModel> favoritos = await _favoritosRepositorio.BuscarFavoritosDoUsuarioAsync(id);

            if (carrinho == null)
            {
                return RedirectToAction("Index", "Home");
            }

            var viewModel = new FavoritoViewModel
            {
                Favoritos = favoritos,
                Carrinho = carrinho
            };

            return View(viewModel);
        }

        public class FavoritoRequest
        {
            public int ProdutoId { get; set; }
        }

        // Método para adicionar um produto aos favoritos
        [HttpPost]
        public async Task<IActionResult> AddOuRemoverFavorito([FromBody] FavoritoRequest request)
        {
            try
            {
                var usuario = _sessao.BuscarSessaoDoUsuario();

                var produto = await _produtoRepositorio.BuscarProdutoPorIdAsync(request.ProdutoId);

                if (produto == null || usuario == null)
                {
                    return Json(new { success = false });
                }

                await _favoritosRepositorio.AddOuRemoverFavoritoAsync(request.ProdutoId, usuario.UsuarioId);

                var novoFavorito = await _favoritosRepositorio.BuscarProdutoExistenteNoFavoritosAsync(request.ProdutoId, usuario.UsuarioId);

                return Json(new
                {
                    success = true,
                    favoritoAtivo = novoFavorito != null // Retorna true ou false dependendo se o usuário curtiu
                });
            }
            catch (Exception ex)
            {
                // Em caso de erro, loga a exceção e retorna um erro genérico
                Console.WriteLine(ex.Message);
                return StatusCode(500, "Erro ao adicionar produto aos favoritos.");
            }
        }
    }
}