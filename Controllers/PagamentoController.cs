using DigitalStore.Helper;
using DigitalStore.Helper.Interfaces;
using DigitalStore.Models;
using DigitalStore.Repositorio.Interfaces;
using DigitalStore.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using DigitalStore.Enums;
using DigitalStore.Filters;

namespace DigitalStore.Controllers
{
    /* Controlador responsável pelo processo de pagamento.
     - Pagamento(int id): Exibe a página de pagamento com os dados do usuário, endereço, e carrinho.
     - PagamentoSucesso(): Exibe a página de sucesso após o pagamento.

    MÉTODOS PRIVADOS NECESSÁRIOS PARA PROCESSAR PAGAMENTO:
     - CriarPedido(int usuarioId, string endereco): Cria um pedido no banco de dados para o usuário.
     - ProcessarItensPedido(int pedidoId, int usuarioId): Processa os itens no carrinho e registra-os como itens do pedido.
     - RegistrarPagamento(PedidoModel pedido, StripeModel stripeModel): Registra o pagamento associado ao pedido no banco de dados.
     - EsvaziarCarrinho(int usuarioId): Remove todos os itens do carrinho do usuário após o pagamento ser processado.
     - EnviarEmailConfirmacao(PedidoModel pedido): Envia um e-mail de confirmação para o usuário após o pagamento ser processado.

    Método Post - ProcessarPagamento:
     - ProcessarPagamento(StripeModel stripeModel): Processa o pagamento através do Stripe, cria o pedido, registra os itens, e envia a confirmação.
    */
    [PaginaCliente]
    public class PagamentoController : Controller
    {
        // Declaração das dependências
        private readonly string _publishableKey;

        // Repositórios
        private readonly ICarrinhoRepositorio _carrinhoRepositorio;
        private readonly IEnderecoRepositorio _enderecoRepositorio;
        private readonly IItensDoPedidoRepositorio _itensDoPedidoRepositorio;
        private readonly IUsuarioRepositorio _usuarioRepositorio;
        private readonly IPagamentoRepositorio _pagamentoRepositorio;
        private readonly IPedidoRepositorio _pedidoRepositorio;
        private readonly IProdutoRepositorio _produtoRepositorio;

        // Serviços e Helpers
        private readonly IEnderecoFrete _enderecoFrete;
        private readonly IEmail _email;
        private readonly ISessao _sessao;
        private readonly StripeService _stripeService;
        private readonly ILogger<PagamentoController> _logger;

        // Construtor com injeção de dependência
        public PagamentoController(IOptions<StripeSettings> stripeSettings, ICarrinhoRepositorio carrinhoRepositorio,
            IEnderecoRepositorio enderecoRepositorio, IItensDoPedidoRepositorio itensDoPedidoRepositorio,
            IUsuarioRepositorio usuarioRepositorio, IPagamentoRepositorio pagamentoRepositorio,
            IPedidoRepositorio pedidoRepositorio, IProdutoRepositorio produtoRepositorio,
            IEnderecoFrete enderecoFrete, IEmail email,
            ISessao sessao, StripeService stripeService,
            ILogger<PagamentoController> logger)
        {
            _publishableKey = stripeSettings?.Value?.PublishableKey ?? throw new ArgumentNullException(nameof(stripeSettings), "PublishableKey não pode ser nulo.");

            // Inicializando repositórios
            _carrinhoRepositorio = carrinhoRepositorio ?? throw new ArgumentNullException(nameof(carrinhoRepositorio), "CarrinhoRepositorio não pode ser nulo.");
            _enderecoRepositorio = enderecoRepositorio ?? throw new ArgumentNullException(nameof(enderecoRepositorio), "EnderecoRepositorio não pode ser nulo.");
            _itensDoPedidoRepositorio = itensDoPedidoRepositorio ?? throw new ArgumentNullException(nameof(itensDoPedidoRepositorio), "ItensDoPedidoRepositorio não pode ser nulo.");
            _usuarioRepositorio = usuarioRepositorio ?? throw new ArgumentNullException(nameof(usuarioRepositorio), "UsuarioRepositorio não pode ser nulo.");
            _pagamentoRepositorio = pagamentoRepositorio ?? throw new ArgumentNullException(nameof(pagamentoRepositorio), "PagamentoRepositorio não pode ser nulo.");
            _pedidoRepositorio = pedidoRepositorio ?? throw new ArgumentNullException(nameof(pedidoRepositorio), "PedidoRepositorio não pode ser nulo.");
            _produtoRepositorio = produtoRepositorio ?? throw new ArgumentNullException(nameof(produtoRepositorio), "ProdutoRepositorio não pode ser nulo.");

            // Inicializando serviços e helpers
            _enderecoFrete = enderecoFrete ?? throw new ArgumentNullException(nameof(enderecoFrete), "EnderecoFrete não pode ser nulo.");
            _email = email ?? throw new ArgumentNullException(nameof(email), "Email não pode ser nulo.");
            _sessao = sessao ?? throw new ArgumentNullException(nameof(sessao), "Sessao não pode ser nulo.");
            _stripeService = stripeService ?? throw new ArgumentNullException(nameof(stripeService), "StripeService não pode ser nulo.");
            _logger = logger ?? throw new ArgumentNullException(nameof(logger), "Logger não pode ser nulo.");
        }


        // Método para exibir a página de pagamento
        public async Task<IActionResult> Pagamento(int id)
        {
            try
            {
                if (id <= 0)
                {
                    throw new ArgumentNullException("ID inválido.");
                }

                // Busca o usuário na sessão
                var usuario = _sessao.BuscarSessaoDoUsuario();
                if (usuario == null)
                {
                    // Lança erro se o usuário não for encontrado
                    throw new Exception("Usuário não encontrado.");
                }

                // Busca o endereço
                var endereco = await _enderecoRepositorio.BuscarEnderecoPorIdAsync(id);
                if (endereco == null)
                {
                    // Lança erro se o endereço não for encontrado
                    throw new Exception("Endereço não encontrado.");
                }

                // Busca o carrinho do usuário
                var carrinho = await _carrinhoRepositorio.BuscarCarrinhoDoUsuarioAsync(usuario.UsuarioId);
                if (carrinho == null || carrinho.Count == 0)
                {
                    TempData["Alerta"] = "Carrinho vazio, adicione produtos e siga para o pagamento.";
                    return RedirectToAction("Carrinho", "Carrinho");
                }

                // Calcula o frete para o endereço fornecido
                var frete = await _enderecoFrete.CalcularFretePorEnderecoAsync(endereco.EnderecoCompleto);

                // Criação do modelo de pagamento
                var viewModel = new PagamentoViewModel
                {
                    CarrinhoTotal = carrinho.Sum(x => x.Produto.Preco * x.Quantidade), // Calcula o total do carrinho
                    Endereco = endereco,
                    Frete = frete,
                    UsuarioId = usuario.UsuarioId,
                    StripeKey = _publishableKey, // Chave de teste do Stripe
                    Valor = carrinho.Sum(x => x.Produto.Preco * x.Quantidade) + frete,  // Valor total do pagamento (carrinho + frete)
                };

                return View(viewModel); // Exibe a view com o modelo de pagamento
            }
            catch (Exception ex)
            {
                // Loga o erro e envia uma mensagem amigável para o usuário
                _logger.LogError(ex, "Erro ao exibir a página de pagamento.");
                TempData["Alerta"] = "Ocorreu um erro ao tentar exibir a página de pagamento. Tente novamente mais tarde.";
                return RedirectToAction("Carrinho", "Carrinho");
            }
        }

        // Página de sucesso após o pagamento
        public IActionResult PagamentoSucesso()
        {
            return View(); // Exibe a página de sucesso
        }

        /*
         * MÉTODOS PRIVADOS NECESSÁRIOS PARA PROCESSAR PAGAMENTO.
         */

        // Método para criar um pedido no banco de dados
        private async Task<PedidoModel> CriarPedido(int usuarioId, string endereco)
        {
            try
            {
                // Busca o usuário pelo ID
                var usuario = await _usuarioRepositorio.BuscarUsuarioPorIdAsync(usuarioId);
                if (usuario == null)
                {
                    throw new InvalidOperationException("Usuário não encontrado.");
                }

                if (string.IsNullOrEmpty(endereco))
                {
                    throw new InvalidOperationException("Endereço é nulo.");
                }


                // Cria um novo pedido
                var pedido = new PedidoModel
                {
                    UsuarioId = usuarioId,
                    Endereco = endereco,
                    DataDoPedido = DateTime.Now,
                    StatusDoPedido = PedidoEnum.Pendente,
                };
                // Adiciona o pedido ao banco de dados
                await _pedidoRepositorio.AddPedidoAsync(pedido);

                return pedido;
            }
            catch (Exception ex)
            {
                // Loga qualquer outra exceção genérica
                _logger.LogError(ex, "Erro inesperado ao criar pedido.");
                TempData["Alerta"] = "Ocorreu um erro ao tentar criar o pedido. Tente novamente mais tarde.";
                return null;
            }
        }

        // Método para processar os itens do pedido no carrinho
        private async Task ProcessarItensPedido(int pedidoId, int usuarioId)
        {
            var carrinho = await _carrinhoRepositorio.BuscarCarrinhoDoUsuarioAsync(usuarioId);
            if (carrinho == null) throw new Exception("Carrinho não encontrado.");

            foreach (var produto in carrinho)
            {
                // Cria o item do pedido com as informações do produto no carrinho
                var itemDoPedido = new ItensDoPedidoModel
                {
                    ProdutoId = produto.ProdutoId,
                    PedidoId = pedidoId,
                    QuantidadeItem = produto.Quantidade,
                    PrecoUnidadeItem = produto.Produto.Preco
                };

                await _itensDoPedidoRepositorio.AddItemAsync(itemDoPedido);

                // Atualiza a quantidade no estoque do produto
                var produtoNoEstoque = await _produtoRepositorio.BuscarProdutoPorIdAsync(produto.ProdutoId);
                produtoNoEstoque.QuantidadeEstoque -= produto.Quantidade;

                await _produtoRepositorio.AtualizarProdutoAsync(produtoNoEstoque); // Atualiza o estoque
            }
        }

        // Método para registrar o pagamento no banco de dados
        private async Task RegistrarPagamento(PedidoModel pedido, StripeModel stripeModel)
        {
            var pagamento = new PagamentoModel
            {
                PedidoId = pedido.PedidoId,
                MetodoPagamento = MetodoPagamentoEnum.Cartao, // Define o método de pagamento
                StatusPagamento = StatusPagamentoEnum.Pago, // Marca o pagamento como 'Pago'
                TokenStripe = stripeModel.Token, // Armazena o token gerado pelo Stripe
                Valor = stripeModel.Valor // Valor total do pagamento
            };

            await _pagamentoRepositorio.AddPagamentoAsync(pagamento); // Adiciona o pagamento ao banco
            pedido.PagamentoId = pagamento.PagamentoId; // Atualiza o pedido com o ID do pagamento
            await _pedidoRepositorio.AtualizarPedidoAsync(pedido); // Atualiza o pedido com o pagamento registrado
        }

        // Método para esvaziar o carrinho do usuário após o pagamento
        private async Task EsvaziarCarrinho(int usuarioId)
        {
            var carrinho = await _carrinhoRepositorio.BuscarCarrinhoDoUsuarioAsync(usuarioId);
            foreach (var item in carrinho)
            {
                // Remove os itens do carrinho
                await _carrinhoRepositorio.AddOuRemoverCarrinhoAsync(item.ProdutoId, usuarioId);
            }
        }

        // Método reponsável por enviar um e-mail de confirmação de pedido/pagamento para o usuário
        private async Task EnviarEmailConfirmacao(PedidoModel pedido)
        {
            var mensagem = "Pedido realizado com sucesso, confira seu pedido em 'Meus Pedidos'";
            await _email.EnviarEmailAsync(pedido.Usuario.Email, "Pedido Realizado - DigitalStore", mensagem);
        }

        // Método para processar o pagamento com Stripe
        // Á função no JavaScript que chama este método é chamada "processarPagamento"
        // E Dependendo do resultado redireciona para view de PagementoSucesso ou exibe um alerta de Erro.
        [HttpPost]
        public async Task<IActionResult> ProcessarPagamento([FromBody] StripeModel stripeModel)
        {
            try
            {
                // Verifica se o modelo de pagamento foi fornecido
                if (stripeModel == null)
                {
                    throw new ArgumentException("Modelo de pagamento está nulo.");
                }

                // Verifica se o token de pagamento é válido
                if (string.IsNullOrEmpty(stripeModel.Token))
                {
                    throw new ArgumentException("Token inválido.");
                }

                // Cria a cobrança através do serviço Stripe
                var charge = _stripeService.CriarCobranca(stripeModel.Token, (int)(stripeModel.Valor * 100), stripeModel.Descricao);

                // Verifica o status da cobrança
                if (charge.Status != "succeeded")
                {
                    throw new Exception("Falha no pagamento.");
                }


                // Adiciona o pedido no banco de dados
                var pedido = await CriarPedido(stripeModel.UsuarioId, stripeModel.EnderecoCompleto);

                await ProcessarItensPedido(pedido.PedidoId, stripeModel.UsuarioId);
                await RegistrarPagamento(pedido, stripeModel);
                await EsvaziarCarrinho(stripeModel.UsuarioId);
                await EnviarEmailConfirmacao(pedido);

                return Json(new { success = true }); // Retorna sucesso
            }
            catch (Exception ex)
            {
                // Loga o erro e envia uma mensagem amigável para o usuário
                _logger.LogError(ex, "Erro ao processar o pagamento.");
                return Json(new { success = false, message = "Ocorreu um erro ao processar o pagamento. Tente novamente mais tarde." });
            }
        }
    }
}
