using DigitalStore.Helper.Interfaces;
using DigitalStore.ViewModels;
using DigitalStore.Repositorio.Interfaces;
using Microsoft.AspNetCore.Mvc;
using DigitalStore.Enums;

namespace DigitalStore.Component
{
    // ViewComponent para exibir a Navbar, usada em _Layout
    public class NavBar : ViewComponent
    {
        private readonly ISessao _sessao;
        private readonly ISiteRepositorio _site;
        private readonly ICarrinhoRepositorio _carrinhoRepositorio;
        private readonly IFavoritosRepositorio _favoritosRepositorio;
        private readonly IProdutoRepositorio _produtoRepositorio;
        private readonly ILogger<NavBar> _logger;

        public NavBar(ISessao sessao,
                        ISiteRepositorio site,
                        ICarrinhoRepositorio carrinhorRepositorio,
                        IFavoritosRepositorio favoritosRepositorio,
                        IProdutoRepositorio produtoRepositorio,
                        ILogger<NavBar> logger)  // Injeção do logger
        {
            _sessao = sessao ?? throw new ArgumentNullException(nameof(sessao), "Sessão não pode ser nula");
            _site = site ?? throw new ArgumentNullException(nameof(site), "Repositório de site não pode ser nulo");
            _carrinhoRepositorio = carrinhorRepositorio ?? throw new ArgumentNullException(nameof(carrinhorRepositorio), "Repositório de carrinho não pode ser nulo");
            _favoritosRepositorio = favoritosRepositorio ?? throw new ArgumentNullException(nameof(favoritosRepositorio), "Repositório de favoritos não pode ser nulo");
            _produtoRepositorio = produtoRepositorio ?? throw new ArgumentNullException(nameof(produtoRepositorio), "Repositório de produtos não pode ser nulo");
            _logger = logger ?? throw new ArgumentNullException(nameof(logger), "Logger não pode ser nulo");
        }

        // Método para invocar a exibição da Navbar
        public async Task<IViewComponentResult> InvokeAsync()
        {
            try
            {
                // Buscando o usuário logado e os dados do site
                var usuarioSessao = _sessao.BuscarSessaoDoUsuario();
                var site = await _site.BuscarDadosDoSiteAsync();

                if (site == null)
                {
                    _logger.LogError("Dados do site não encontrados.");
                    throw new InvalidOperationException("Não foi possível carregar os dados do site.");
                }

                // Criando o ViewModel para a Navbar
                var viewModel = new NavBarViewModel
                {
                    UsuarioLogado = usuarioSessao != null,
                    UsuarioLogadoId = usuarioSessao?.UsuarioId ?? 0,
                    PerfilUsuarioCliente = usuarioSessao?.Perfil == PerfilEnum.Cliente,
                    NomeSite = site.NomeSite,
                    Categorias = await _produtoRepositorio.BuscarCategoriasAsync(),
                    CarrinhoQuantidadeDeProdutos = usuarioSessao != null
                        ? (await _carrinhoRepositorio.BuscarCarrinhoDoUsuarioAsync(usuarioSessao.UsuarioId)).Count
                        : 0,
                    FavoritosQuantidadeDeProdutos = usuarioSessao != null
                        ? (await _favoritosRepositorio.BuscarFavoritosDoUsuarioAsync(usuarioSessao.UsuarioId)).Count
                        : 0
                };

                return View(viewModel);
            }
            catch (Exception ex)
            {
                // Logando o erro
                _logger.LogError(ex, "Erro ao carregar a Navbar.");

                // Exibe um alerta de erro ao usuário e redireciona para página de Login.
                TempData["Alerta"] = "Desculpe, Ocorreu um erro ao carregar a página.";
                return View("Login", "Login");
            }
        }
    }
}
