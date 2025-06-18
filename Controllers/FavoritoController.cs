using DigitalStore.Helper.Interfaces;
using DigitalStore.ViewModels;
using DigitalStore.Repositorio.Interfaces;
using Microsoft.AspNetCore.Mvc;
using DigitalStore.Filters;

namespace DigitalStore.Controllers
{
    /* 
     Controlador responsável pela gestão dos favoritos de um usuário.
     - Favoritos(int id): Exibe os favoritos e o carrinho de um usuário.
     - AddOuRemoverFavorito(FavoritoRequest request): Adiciona ou remove um produto dos favoritos do usuário.
    */
    [PaginaCliente]
    public class FavoritoController : Controller
    {
        // Declaração das dependências dos repositórios
        private readonly IFavoritosRepositorio _favoritosRepositorio;
        private readonly IProdutoRepositorio _produtoRepositorio;
        private readonly ICarrinhoRepositorio _carrinhoRepositorio;
        private readonly ISessao _sessao;
        private readonly ILogger<FavoritoController> _logger; // ILogger para logar erros e eventos.

        // Construtor para injeção de dependências
        public FavoritoController(IFavoritosRepositorio favoritosRepositorio,
                                  IProdutoRepositorio produtoRepositorio,
                                  ICarrinhoRepositorio carrinhoRepositorio,
                                  ISessao sessao,
                                  ILogger<FavoritoController> logger)
        {
            _favoritosRepositorio = favoritosRepositorio ?? throw new ArgumentNullException(nameof(favoritosRepositorio));
            _produtoRepositorio = produtoRepositorio ?? throw new ArgumentNullException(nameof(produtoRepositorio));
            _carrinhoRepositorio = carrinhoRepositorio ?? throw new ArgumentNullException(nameof(carrinhoRepositorio));
            _sessao = sessao ?? throw new ArgumentNullException(nameof(sessao));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        // Método para exibir os favoritos e o carrinho do usuário
        public async Task<IActionResult> Favoritos(int id)
        {
            try
            {
                if (id <= 0) // Verifica se o ID é inválido (zero ou negativo)
                {
                    throw new ArgumentException($"ID do usuário inválido: {id}");
                }

                var carrinho = await _carrinhoRepositorio.BuscarCarrinhoDoUsuarioAsync(id);
                var favoritos = await _favoritosRepositorio.BuscarFavoritosDoUsuarioAsync(id);

                if (carrinho == null || favoritos == null)
                {
                    throw new InvalidOperationException($"Carrinho ou favoritos do usuário {id} não encontrados.");
                }

                // Cria um objeto ViewModel para passar os dados para a View
                var viewModel = new FavoritoViewModel
                {
                    Favoritos = favoritos,
                    Carrinho = carrinho
                };

                return View(viewModel);
            }
            catch (Exception ex)
            {
                // Loga o erro caso ocorra algum problema ao tentar buscar os dados
                _logger.LogError(ex, "Erro ao buscar favoritos e carrinho do usuário.");
                TempData["Alerta"] = "Ocorreu um erro ao tentar carregar seus favoritos. Tente novamente mais tarde.";
                return RedirectToAction("Index", "Home");
            }
        }

        // Classe de requisição para adicionar ou remover favorito
        public class FavoritoRequest
        {
            public int ProdutoId { get; set; }
        }

        // Método para adicionar ou remover um produto dos favoritos, via requisição AJAX
        [HttpPost]
        public async Task<IActionResult> AddOuRemoverFavorito([FromBody] FavoritoRequest request)
        {
            try
            {
                if (request.ProdutoId <= 0)
                {
                    // Loga se o produto ou o usuário não forem encontrados
                    throw new ArgumentException($"Produto não encontrado para a requisição de favorito. ProdutoId: {request.ProdutoId}");
                }

                // Busca o usuário logado na sessão
                var usuario = _sessao.BuscarSessaoDoUsuario();

                // Verifica se o produto e o usuário são válidos
                var produto = await _produtoRepositorio.BuscarProdutoPorIdAsync(request.ProdutoId);
                if (produto == null || usuario == null)
                {
                    // Loga se o produto ou o usuário não forem encontrados
                    throw new InvalidOperationException($"Produto ou usuário não encontrado para a requisição de favorito. ProdutoId: {request.ProdutoId}");
                }

                // Realiza a operação de adicionar ou remover favorito
                await _favoritosRepositorio.AddOuRemoverFavoritoAsync(request.ProdutoId, usuario.UsuarioId);

                // Verifica se o produto foi adicionado aos favoritos
                var novoFavorito = await _favoritosRepositorio.BuscarProdutoExistenteNoFavoritosAsync(request.ProdutoId, usuario.UsuarioId);

                _logger.LogInformation($"Produto {request.ProdutoId} {(novoFavorito != null ? "adicionado" : "removido")} aos favoritos do usuário {usuario.UsuarioId}.");

                // Retorna o status da operação junto com o novo estado do favorito
                return Json(new
                {
                    success = true,
                    favoritoAtivo = novoFavorito != null // Retorna true ou false dependendo se o usuário curtiu
                });
            }
            catch (Exception ex)
            {
                // Loga qualquer exceção que ocorra e retorna um erro genérico
                _logger.LogError(ex, "Erro ao adicionar ou remover produto dos favoritos.");
                return Json(new
                {
                    success = false,
                    message = $"Erro ao adicionar ou remover produto dos favoritos. Detalhes: {ex.Message}"
                });
            }
        }
    }
}
