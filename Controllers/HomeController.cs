using DigitalStore.Helper.Interfaces;
using DigitalStore.Enums;
using DigitalStore.Models;
using DigitalStore.ViewModels;
using DigitalStore.Repositorio.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace DigitalStore.Controllers
{
    // Controlador respons�vel pela gest�o dos produtos e intera��es da p�gina inicial.
    // M�todos:
    // - ObterHomeViewModel(): M�todo auxiliar que retorna um modelo com as informa��es da p�gina inicial,
    //   como status do usu�rio logado, seus favoritos e itens no carrinho.
    // - Categoria(string categoria): Exibe os produtos de uma determinada categoria.
    // - Error: Exibe a p�gina de erro.
    // - Index(): Carrega a p�gina inicial com todos os produtos, �ltimas novidades e dados do site.
    // - Produto(int id): Exibe os detalhes de um produto espec�fico.
    // - Suporte(): Exibe a p�gina de suporte ao usu�rio.
    // - TodosOsProdutos(): Exibe todos os produtos cadastrados.
    // - UltimasNovidades(): Exibe as �ltimas novidades com os produtos rec�m-adicionados.
    // - (GET) BarraDePesquisa(string termo): Realiza uma busca de produtos com base no termo fornecido.
    // - (POST) EnviarNewsletter(string email): Envia uma newsletter promocional para o email informado.

    public class HomeController : Controller
    {
        // Declara��o de depend�ncias para reposit�rios e servi�os utilizados na classe.
        private readonly ICarrinhoRepositorio _carrinhoRepositorio;
        private readonly IFavoritosRepositorio _favoritosRepositorio;
        private readonly IProdutoRepositorio _produtoRepositorio;
        private readonly ISiteRepositorio _siteRepositorio;
        private readonly ISessao _sessao;
        private readonly IEmail _email;
        private readonly ILogger<HomeController> _logger;

        // Construtor que injeta as depend�ncias necess�rias para os m�todos
        public HomeController(ICarrinhoRepositorio carrinhoRepositorio,
                        IFavoritosRepositorio favoritosRepositorio,
                        IProdutoRepositorio produtoRepositorio,
                        ISiteRepositorio siteRepositorio,
                        ISessao sessao,
                        IEmail email,
                        ILogger<HomeController> logger)
        {
            _carrinhoRepositorio = carrinhoRepositorio ?? throw new ArgumentNullException(nameof(carrinhoRepositorio));
            _favoritosRepositorio = favoritosRepositorio ?? throw new ArgumentNullException(nameof(favoritosRepositorio));
            _produtoRepositorio = produtoRepositorio ?? throw new ArgumentNullException(nameof(produtoRepositorio));
            _siteRepositorio = siteRepositorio ?? throw new ArgumentNullException(nameof(siteRepositorio));
            _sessao = sessao ?? throw new ArgumentNullException(nameof(sessao));
            _email = email ?? throw new ArgumentNullException(nameof(email));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        // M�todo auxiliar que monta o ViewModel com informa��es sobre o usu�rio, seus favoritos e carrinho
        private async Task<HomeViewModel> ObterHomeViewModel()
        {
            // Busca a sess�o do usu�rio
            var usuarioSessao = _sessao.BuscarSessaoDoUsuario();

            // Carrega os favoritos e o carrinho, se o usu�rio estiver logado
            var favoritos = usuarioSessao?.UsuarioId > 0
                ? await _favoritosRepositorio.BuscarFavoritosDoUsuarioAsync(usuarioSessao.UsuarioId)
                : new List<FavoritosModel>();

            var carrinho = usuarioSessao?.UsuarioId > 0
                ? await _carrinhoRepositorio.BuscarCarrinhoDoUsuarioAsync(usuarioSessao.UsuarioId)
                : new List<CarrinhoModel>();

            // Retorna um HomeViewModel preenchido com as informa��es do usu�rio
            return new HomeViewModel
            {
                UsuarioLogado = usuarioSessao?.UsuarioId > 0,
                PerfilUsuarioCliente = usuarioSessao?.Perfil == PerfilEnum.Cliente,
                FavoritosDoUsuario = favoritos,
                CarrinhoDoUsuario = carrinho
            };
        }

        // M�todo para exibir produtos por categoria
        public async Task<IActionResult> Categoria(string categoria)
        {
            try
            {
                // Valida se a categoria fornecida � v�lida
                if (string.IsNullOrEmpty(categoria))
                {
                    throw new ArgumentException("Categoria inv�lida.", nameof(categoria));
                }

                // Obter o ViewModel com dados do usu�rio e outras informa��es
                var viewModel = await ObterHomeViewModel();

                // Carregar os produtos da categoria selecionada
                var produtosCategoria = await _produtoRepositorio.BuscarProdutosPorCategoriaAsync(categoria);

                // Verificar se a lista de produtos foi retornada corretamente
                if (produtosCategoria == null)
                {
                    throw new ArgumentException("Lista de produtos da categoria retornou null");
                }

                // Preencher o ViewModel com os produtos encontrados
                viewModel.Produtos = produtosCategoria;

                return View(viewModel);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao carregar produtos da categoria.");
                TempData["Alerta"] = "Houve um erro ao tentar acessar a p�gina";
                return RedirectToAction("Index");
            }
        }

        // Exibe a p�gina de erro
        public IActionResult Error()
        {
            return View();
        }

        // M�todo para carregar a p�gina inicial com todos os produtos
        public async Task<IActionResult> Index()
        {
            try
            {
                // Obter o ViewModel com dados do usu�rio e outras informa��es
                var viewModel = await ObterHomeViewModel();

                // Carregar os dados adicionais, como todos os produtos, dados do site e �ltimas novidades
                viewModel.Produtos = await _produtoRepositorio.BuscarTodosOsProdutosAsync();
                viewModel.SiteDados = await _siteRepositorio.BuscarDadosDoSiteAsync();
                viewModel.UltimasNovidades = await _produtoRepositorio.BuscarUltimosProdutosAdicionadosAsync();

                return View(viewModel);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao carregar a p�gina inicial.");
                TempData["Alerta"] = "Ocorreu um erro ao carregar a p�gina inicial.";
                return RedirectToAction("Login", "Login");
            }
        }

        // M�todo para exibir detalhes de um produto espec�fico
        public async Task<IActionResult> Produto(int id)
        {
            try
            {
                // Verifica se o id do produto � v�lido
                if (id <= 0)
                {
                    throw new ArgumentException("Id inv�lido.", nameof(id));
                }

                // Buscar o produto no reposit�rio
                var produto = await _produtoRepositorio.BuscarProdutoPorIdAsync(id);

                // Verifica se o produto foi encontrado
                if (produto == null)
                {
                    throw new ArgumentException("Produto n�o encontrado.");
                }

                // Obter o ViewModel e adicionar os detalhes do produto
                var viewModel = await ObterHomeViewModel();
                viewModel.Produto = produto;
                viewModel.Produtos = await _produtoRepositorio.BuscarUltimosProdutosAdicionadosAsync();

                return View(viewModel);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao carregar detalhes do produto.");
                TempData["Alerta"] = "Houve um erro ao tentar acessar a p�gina";
                return RedirectToAction("Index");
            }
        }

        // M�todo para exibir p�gina de suporte
        public IActionResult Suporte()
        {
            return View();
        }

        // M�todo para listar todos os produtos
        public async Task<IActionResult> TodosOsProdutos()
        {
            try
            {
                var viewModel = await ObterHomeViewModel();

                // Buscar todos os produtos
                viewModel.Produtos = await _produtoRepositorio.BuscarTodosOsProdutosAsync();

                return View(viewModel);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao listar todos os produtos.");
                TempData["Alerta"] = "Ocorreu um erro ao tentar acessar essa p�gina.";
                return RedirectToAction("Index");
            }
        }

        // M�todo para exibir �ltimas novidades
        public async Task<IActionResult> UltimasNovidades()
        {
            try
            {
                var viewModel = await ObterHomeViewModel();

                // Carregar as �ltimas novidades
                viewModel.Produtos = await _produtoRepositorio.BuscarUltimosProdutosAdicionadosAsync();

                return View(viewModel);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao listar �ltimas novidades.");
                TempData["Alerta"] = "Ocorreu um erro ao tentar acessar essa p�gina.";
                return RedirectToAction("Index");
            }
        }

        // M�todo para realizar busca de produtos por termo, arquivo js barra-de-pesquisa.js
        [HttpGet]
        public async Task<IActionResult> BarraDePesquisa(string termo)
        {
            if (string.IsNullOrEmpty(termo))
            {
                return Json(new List<object>());
            }

            // Buscar produtos ou categorias com base no termo de pesquisa
            var produtosOuCategoria = await _produtoRepositorio.BuscarProdutosBarraDePesquisaAsync(termo);

            // Retornar os resultados da busca como um JSON
            var resultados = produtosOuCategoria.Select(x => new
            {
                id = x.ProdutoId,
                nome = $"{x.NomeProduto} - {x.Categoria}"  // Formatar o nome para exibir produto e categoria
            }).ToList();

            return Json(resultados);
        }

        // M�todo para enviar newsletter por email
        [HttpPost]
        public async Task<IActionResult> EnviarNewsletter(string email)
        {
            try
            {
                // Valida o email para garantir que n�o seja nulo
                if (string.IsNullOrEmpty(email))
                {
                    throw new ArgumentNullException("Email � nulo");
                }

                var mensagem = "Enviaremos cupons promocionais em breve. Fique de olho nas novidades!";

                // Enviar o email com a mensagem de newsletter
                var emailEnviado = await _email.EnviarEmailAsync(email, "Newsletter - DigitalStore", mensagem);

                if (!emailEnviado)
                {
                    return Json(new { success = false, message = "Erro no envio da newsletter" });
                }

                return Json(new { success = true, message = "Newsletter enviada com sucesso" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Houve um erro no envio da newsletter");
                return Json(new { success = false, message = "Erro no envio da newsletter" });
            }
        }
    }
}
