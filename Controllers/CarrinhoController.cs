using DigitalStore.Helper.Interfaces;
using DigitalStore.Models;
using DigitalStore.ViewModels;
using DigitalStore.Repositorio.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using DigitalStore.Helper;
using DigitalStore.Filters;

namespace DigitalStore.Controllers
{
    /*
        Controlador responsável pelo gerenciamento do carrinho de compras.
        Possui métodos para exibir o carrinho, adicionar/remover produtos e atualizar quantidades.
        Métodos:
        - Metodo Get para View Carrinho():Retorna os itens do carrinho e os endereços do usuário.
        - (POST) AddOuRemoverCarrinho(CarrinhoRequest request): Adiciona ou remove um produto do carrinho via requisição AJAX.
        - (POST) AtualizarQuantidade(int produtoId, int usuarioId, string acao): Atualiza a quantidade de um produto no carrinho.
    */
    [PaginaCliente]
    public class CarrinhoController : Controller
    {
        // Dependências do repositório e logger
        private readonly  string _googleApiKey;
        private readonly ICarrinhoRepositorio _carrinhoRepositorio;
        private readonly IProdutoRepositorio _produtoRepositorio;
        private readonly IEnderecoRepositorio _enderecoRepositorio;
        private readonly ISessao _sessao;
        private readonly ILogger<CarrinhoController> _logger;

        // Construtor com injeção de dependência
        public CarrinhoController(IOptions<GoogleAPISettings> googleApiKey,
            ICarrinhoRepositorio carrinhoRepositorio,
            IProdutoRepositorio produtoRepositorio,
            IEnderecoRepositorio enderecoRepositorio,
            ISessao sessao,
            ILogger<CarrinhoController> logger)
        {
            _googleApiKey = googleApiKey.Value.ApiKey ?? throw new ArgumentNullException(nameof(googleApiKey.Value.ApiKey));
            _carrinhoRepositorio = carrinhoRepositorio ?? throw new ArgumentNullException(nameof(carrinhoRepositorio));
            _produtoRepositorio = produtoRepositorio ?? throw new ArgumentNullException(nameof(produtoRepositorio));
            _enderecoRepositorio = enderecoRepositorio ?? throw new ArgumentNullException(nameof(enderecoRepositorio));
            _sessao = sessao ?? throw new ArgumentNullException(nameof(sessao));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        // Método para exibir o carrinho de um usuário específico
        // Uso de ViewModel para exibir os dados do carrinho e endereços
        public async Task<IActionResult> Carrinho()
        {
            try
            {
                // Busca a sessão do usuário logado.
                var usuario = _sessao.BuscarSessaoDoUsuario();
                if (usuario == null)
                {
                    throw new ArgumentException("Sessao do usuário é nula");
                }

                // Busca os itens do carrinho do usuário no repositório, retorna uma lista vazia se não houver itens
                var carrinho = await _carrinhoRepositorio.BuscarCarrinhoDoUsuarioAsync(usuario.UsuarioId);
                if (carrinho == null)
                {
                    throw new ArgumentException("Carrinho do usuario retornou null");
                }

                // Inicializa um endereço vazio para o usuário
                var endereco = new EnderecoModel
                {
                    UsuarioId = usuario.UsuarioId,
                    EnderecoCompleto = string.Empty
                };

                // Prepara a ViewModel com os dados do carrinho e endereços
                var viewModel = new CarrinhoViewModel
                {
                    GoogleApiKey = _googleApiKey,
                    Endereco = endereco,
                    Enderecos = await _enderecoRepositorio.BuscarTodosOsEnderecosDoUsuarioAsync(usuario.UsuarioId),
                    Carrinho = carrinho
                };

                return View(viewModel);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao carregar o carrinho do usuário");
                TempData["Alerta"] = "Houve um erro ao carregar o carrinho.";
                return RedirectToAction("Index", "Home");
            }
        }

        // Necessário para requisição de adicionar ou remover um produto do carrinho
        public class CarrinhoRequest
        {
            public int ProdutoId { get; set; }
        }

        // Método para adicionar ou remover um produto do carrinho, usando requisição AJAX
        [HttpPost]
        public async Task<IActionResult> AddOuRemoverCarrinho([FromBody] CarrinhoRequest request)
        {
            try
            {
                // Verifica se o ProdutoId é válido
                if (request.ProdutoId <= 0 || await _produtoRepositorio.BuscarProdutoPorIdAsync(request.ProdutoId) == null)
                {
                    throw new ArgumentException($"Produto inválido ou não encontrado: {request.ProdutoId}");
                }

                // Busca o usuário da sessão
                var usuario = _sessao.BuscarSessaoDoUsuario();
                if (usuario == null)
                {
                    throw new ArgumentException("Usuário não encontrado na sessão.");
                }

                // Adiciona ou remove o produto do carrinho
                await _carrinhoRepositorio.AddOuRemoverCarrinhoAsync(request.ProdutoId, usuario.UsuarioId);

                // Verifica se o produto foi adicionado ou removido do carrinho
                var novoCarrinho = await _carrinhoRepositorio.BuscarProdutoExistenteNoCarrinhoAsync(request.ProdutoId, usuario.UsuarioId);

                // Retorna o resultado da operação
                return Json(new { success = true, carrinhoAtivo = novoCarrinho != null });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao adicionar ou remover produto {ProdutoId} do carrinho", request.ProdutoId);
                return Json(new { success = false, message = "Não foi possível executar essa ação." });
            }
        }

        // Necessário para requisição de aumentar ou diminuir a quantidade de um produto no carrinho
        public class AtualizarQuantidadeRequest
        {
            public int ProdutoId { get; set; }
            public required string Acao { get; set; } // Aumentar ou Diminuir
        }

        // Método para atualizar a quantidade de um produto no carrinho, usando requisição AJAX
        [HttpPost]
        public async Task<IActionResult> AtualizarQuantidade([FromBody] AtualizarQuantidadeRequest request)
        {
            try
            {
                // Busca o usuário logado na sessão
                var usuarioLogado = _sessao.BuscarSessaoDoUsuario();

                // Valida se o ProdutoId é válido e se o usuário está logado
                if (request.ProdutoId == 0 || usuarioLogado == null)
                {
                    return Json(new { success = false, message = "ID do Produto ou usuário inválido" });
                }

                // Busca o produto no carrinho do usuário
                var produtoNoCarrinho = await _carrinhoRepositorio.BuscarProdutoExistenteNoCarrinhoAsync(request.ProdutoId, usuarioLogado.UsuarioId);
                if (produtoNoCarrinho == null)
                {
                    return Json(new { success = false, message = "Produto não encontrado no carrinho" });
                }

                // Busca o produto no estoque
                var produtoEstoque = await _produtoRepositorio.BuscarProdutoPorIdAsync(request.ProdutoId);
                if (produtoEstoque == null)
                {
                    return Json(new { success = false, message = "Produto não encontrado." });
                }

                // Verifica a ação solicitada pelo usuário
                switch (request.Acao.ToLower()) // Torna a comparação insensível a maiúsculas e minúsculas
                {
                    case "aumentar":
                        // Verifica se há estoque disponível antes de aumentar a quantidade
                        if (produtoNoCarrinho.Quantidade < produtoEstoque.QuantidadeEstoque)
                        {
                            produtoNoCarrinho.Quantidade ++;
                        }
                        else
                        {
                            return Json(new { success = false, message = "Não há mais estoque disponível para aumentar a quantidade." });
                        }
                        break;

                    case "diminuir":
                        // Garante que a quantidade do produto não seja menor que 1
                        if (produtoNoCarrinho.Quantidade > 1)
                        {
                            produtoNoCarrinho.Quantidade --;
                        }
                        else
                        {
                            return Json(new { success = false });
                        }
                        break;

                    default:
                        return Json(new { success = false, message = "Ação inválida." });
                }

                // Atualiza a quantidade do produto no carrinho do usuário
                await _carrinhoRepositorio.AtualizarQuantidadeAsync(request.ProdutoId, usuarioLogado.UsuarioId, produtoNoCarrinho.Quantidade);

                // Retorna a nova quantidade do produto no carrinho
                return Json(new { success = true, novaQuantidade = produtoNoCarrinho.Quantidade });
            }
            catch (Exception ex)
            {
                // Registra o erro no log e retorna uma mensagem de erro genérica
                _logger.LogError(ex, "Erro ao atualizar a quantidade do produto no carrinho do usuário");
                return Json(new { success = false, message = "Erro ao atualizar a quantidade." });
            }
        }
    }
}
