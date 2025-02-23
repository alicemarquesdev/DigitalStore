using DigitalStore.Helper;
using DigitalStore.Models;
using DigitalStore.Models.ViewModels;
using DigitalStore.Repositorio.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace DigitalStore.Controllers
{
    public class HomeController : Controller
    {
        private readonly ICarrinhoRepositorio _carrinhoRepositorio;
        private readonly IFavoritosRepositorio _favoritosRepositorio;
        private readonly IProdutoRepositorio _produtoRepositorio;
        private readonly ISessao _sessao;
        private readonly ISiteRepositorio _siteRepositorio;

        public HomeController(
            IProdutoRepositorio produtoRepositorio,
            ISessao sessao,
            ICarrinhoRepositorio carrinhoRepositorio,
            IFavoritosRepositorio favoritosRepositorio,
            ISiteRepositorio siteRepositorio)
        {
            _produtoRepositorio = produtoRepositorio;
            _sessao = sessao;
            _carrinhoRepositorio = carrinhoRepositorio;
            _favoritosRepositorio = favoritosRepositorio;
            _siteRepositorio = siteRepositorio;
        }

        // Método para exibir produtos por categoria
        public async Task<IActionResult> Categoria(string categoria)
        {
            try
            {
                var usuarioSessao = _sessao.BuscarSessaoDoUsuario();

                var viewModel = new HomeViewModel
                {
                    Produtos = await _produtoRepositorio.BuscarProdutosPorCategoriaAsync(categoria),
                    UsuarioLogado = usuarioSessao?.UsuarioId > 0,
                    PerfilUsuarioCliente = usuarioSessao?.Perfil == Enum.PerfilEnum.Cliente,
                    FavoritosDoUsuario = usuarioSessao?.UsuarioId > 0
                        ? await _favoritosRepositorio.BuscarFavoritosDoUsuarioAsync(usuarioSessao.UsuarioId)
                        : new List<FavoritosModel>(),
                    CarrinhoDoUsuario = usuarioSessao?.UsuarioId > 0
                        ? await _carrinhoRepositorio.BuscarCarrinhoDoUsuarioAsync(usuarioSessao.UsuarioId)
                        : new List<CarrinhoModel>()
                };

                return View(viewModel);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return StatusCode(500, "Erro ao carregar a página inicial.");
            }
        }

        // Método para carregar a página inicial com todos os produtos
        public async Task<IActionResult> Index()
        {
            try
            {
                var usuarioSessao = _sessao.BuscarSessaoDoUsuario();

                var viewModel = new HomeViewModel
                {
                    Produtos = await _produtoRepositorio.BuscarTodosProdutosAsync(),
                    SiteDados = await _siteRepositorio.BuscarDadosDoSiteAsync(),
                    UltimasNovidades = await _produtoRepositorio.BuscarUltimosProdutosAdicionados(),
                    UsuarioLogado = usuarioSessao?.UsuarioId > 0,
                    PerfilUsuarioCliente = usuarioSessao?.Perfil == Enum.PerfilEnum.Cliente,
                    FavoritosDoUsuario = usuarioSessao?.UsuarioId > 0 ? await _favoritosRepositorio.BuscarFavoritosDoUsuarioAsync(usuarioSessao.UsuarioId) : new List<FavoritosModel>(),
                    CarrinhoDoUsuario = usuarioSessao?.UsuarioId > 0 ? await _carrinhoRepositorio.BuscarCarrinhoDoUsuarioAsync(usuarioSessao.UsuarioId) : new List<CarrinhoModel>()
                };

                return View(viewModel);
            }
            catch (Exception ex)
            {
                TempData["MensagemErro"] = "Ocorreu um erro ao carregar a página inicial.";
                Console.WriteLine(ex);
                return View();
            }
        }

        // Método para exibir detalhes de um produto específico
        public async Task<IActionResult> Produto(int id)
        {
            try
            {
                var usuarioSessao = _sessao.BuscarSessaoDoUsuario();

                var produto = await _produtoRepositorio.BuscarProdutoPorIdAsync(id);

                if (produto == null)
                {
                    TempData["MensagemErro"] = "Produto não encontrado.";
                    return View();
                }

                var viewModel = new HomeViewModel
                {
                    Produto = produto,
                    Produtos = await _produtoRepositorio.BuscarUltimosProdutosAdicionados(),
                    UsuarioLogado = usuarioSessao?.UsuarioId > 0,
                    PerfilUsuarioCliente = usuarioSessao?.Perfil == Enum.PerfilEnum.Cliente,
                    FavoritosDoUsuario = usuarioSessao?.UsuarioId > 0
                     ? await _favoritosRepositorio.BuscarFavoritosDoUsuarioAsync(usuarioSessao.UsuarioId)
                     : new List<FavoritosModel>(),
                    CarrinhoDoUsuario = usuarioSessao?.UsuarioId > 0
                     ? await _carrinhoRepositorio.BuscarCarrinhoDoUsuarioAsync(usuarioSessao.UsuarioId)
                     : new List<CarrinhoModel>()
                };

                return View(viewModel);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                TempData["MensagemErro"] = "Erro ao carregar o produto.";
                return View();
            }
        }

        // Método para exibir página de suporte
        public IActionResult Suporte()
        {
            return View();
        }

        // Método para listar todos os produtos
        public async Task<IActionResult> TodosOsProdutos()
        {
            try
            {
                var usuarioSessao = _sessao.BuscarSessaoDoUsuario();

                var viewModel = new HomeViewModel
                {
                    Produtos = await _produtoRepositorio.BuscarTodosProdutosAsync(),
                    UsuarioLogado = usuarioSessao?.UsuarioId > 0,
                    PerfilUsuarioCliente = usuarioSessao?.Perfil == Enum.PerfilEnum.Cliente,
                    FavoritosDoUsuario = usuarioSessao?.UsuarioId > 0 ? await _favoritosRepositorio.BuscarFavoritosDoUsuarioAsync(usuarioSessao.UsuarioId) : new List<FavoritosModel>(),
                    CarrinhoDoUsuario = usuarioSessao?.UsuarioId > 0 ? await _carrinhoRepositorio.BuscarCarrinhoDoUsuarioAsync(usuarioSessao.UsuarioId) : new List<CarrinhoModel>()
                };

                return View(viewModel);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                TempData["MensagemErro"] = "Ocorreu um erro ao listar todos os produtos.";
                return View();
            }
        }

        // Método para exibir últimas novidades
        public async Task<IActionResult> UltimasNovidades()
        {
            try
            {
                var usuarioSessao = _sessao.BuscarSessaoDoUsuario();

                var viewModel = new HomeViewModel
                {
                    Produtos = await _produtoRepositorio.BuscarUltimosProdutosAdicionados(),
                    UsuarioLogado = usuarioSessao?.UsuarioId > 0,
                    PerfilUsuarioCliente = usuarioSessao?.Perfil == Enum.PerfilEnum.Cliente,
                    FavoritosDoUsuario = usuarioSessao?.UsuarioId > 0
                    ? await _favoritosRepositorio.BuscarFavoritosDoUsuarioAsync(usuarioSessao.UsuarioId)
                    : new List<FavoritosModel>(),
                    CarrinhoDoUsuario = usuarioSessao?.UsuarioId > 0
                    ? await _carrinhoRepositorio.BuscarCarrinhoDoUsuarioAsync(usuarioSessao.UsuarioId)
                    : new List<CarrinhoModel>()
                };

                return View(viewModel);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                TempData["MensagemErro"] = "Erro ao carregar as últimas novidades.";
                return View();
            }
        }

        public IActionResult Error()
        {
            // Você pode capturar mais informações sobre o erro aqui se precisar
            var errorViewModel = new ErrorViewModel
            {
                RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier,
                Message = "Ocorreu um erro ao processar sua solicitação.",
                StatusCode = 500 // Aqui você pode configurar o código do status de erro
            };

            return View(errorViewModel);
        }
    }
}