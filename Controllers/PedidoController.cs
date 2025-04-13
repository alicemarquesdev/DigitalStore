using DigitalStore.Enums;
using DigitalStore.Filters;
using DigitalStore.Helper.Interfaces;
using DigitalStore.Repositorio.Interfaces;
using DigitalStore.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace DigitalStore.Controllers
{
    // Controlador responsável pela gestão dos pedidos de um usuário.
    // - MeusPedidos(): Exibe todos os pedidos do usuário logado.
    // - PedidosEPagamentos(): Exibe todos os pedidos e pagamentos para o ADMIN.
    // - AtualizarStatusDoPedido(): Atualiza o status do pedido, ADMIN.
    public class PedidoController : Controller
    {
        // Declaração das dependências dos repositórios e sessão do usuário.
        private readonly IItensDoPedidoRepositorio _itensDoPedidoRepositorio;
        private readonly IPedidoRepositorio _pedidoRepositorio;
        private readonly IEnderecoRepositorio _enderecoRepositorio;
        private readonly IUsuarioRepositorio _usuarioRepositorio;
        private readonly ICarrinhoRepositorio _carrinhoRepositorio;
        private readonly IPagamentoRepositorio _pagamentoRepositorio;
        private readonly ISessao _sessao;
        private readonly ILogger<PedidoController> _logger; // ILogger para logar erros e eventos.

        // Construtor para injeção de dependências
        public PedidoController(IItensDoPedidoRepositorio itensDoPedidoRepositorio,
                                IPedidoRepositorio pedidoRepositorio,
                                IEnderecoRepositorio enderecoRepositorio,
                                IUsuarioRepositorio usuarioRepositorio,
                                ICarrinhoRepositorio carrinhoRepositorio,
                                IPagamentoRepositorio pagamentoRepositorio,
                                ISessao sessao,
                                ILogger<PedidoController> logger)
        {
            _itensDoPedidoRepositorio = itensDoPedidoRepositorio ?? throw new ArgumentNullException(nameof(itensDoPedidoRepositorio));
            _pedidoRepositorio = pedidoRepositorio ?? throw new ArgumentNullException(nameof(pedidoRepositorio));
            _usuarioRepositorio = usuarioRepositorio ?? throw new ArgumentNullException(nameof(usuarioRepositorio));
            _carrinhoRepositorio = carrinhoRepositorio ?? throw new ArgumentNullException(nameof(carrinhoRepositorio));
            _enderecoRepositorio = enderecoRepositorio ?? throw new ArgumentNullException(nameof(enderecoRepositorio));
            _pagamentoRepositorio = pagamentoRepositorio ?? throw new ArgumentNullException(nameof(pagamentoRepositorio));
            _sessao = sessao ?? throw new ArgumentNullException(nameof(sessao));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        // Método para exibir todos os pedidos do usuário logado.
        [PaginaCliente]
        public async Task<IActionResult> MeusPedidos()
        {
            try
            {
                // Verifica se o usuário está logado, se não redireciona para a página de login
                var usuario = _sessao.BuscarSessaoDoUsuario();
                if (usuario == null)
                {
                    throw new ArgumentNullException(nameof(usuario), "Usuário não encontrado na sessão.");
                }

                // Busca os pedidos do usuário
                var pedidos = await _pedidoRepositorio.BuscarTodosOsPedidosDoUsuarioAsync(usuario.UsuarioId);

                if (pedidos == null)
                {
                    throw new InvalidOperationException("Nenhum pedido encontrado para este usuário.");
                }

                var viewModel = new PedidoViewModel
                {
                    UsuarioId = usuario.UsuarioId,
                    Pedidos = pedidos
                };

                // Retorna a view com os pedidos
                return View(viewModel);
            }
            catch (Exception ex)
            {
                // Loga qualquer outra exceção genérica
                _logger.LogError(ex, "Erro inesperado ao buscar pedidos.");
                TempData["Alerta"] = "Ocorreu um erro ao tentar carregar seus pedidos. Tente novamente mais tarde.";
                return RedirectToAction("Index", "Home");
            }
        }

        // Método para exibir todos os pedidos e pagamentos para o ADMIN.
        [PaginaAdmin]
        public async Task<IActionResult> PedidosEPagamentos()
        {
            try
            {
                // Busca todos os pedidos
                var pedidos = await _pedidoRepositorio.BuscarTodosOsPedidosAsync();
                if (pedidos == null)
                {
                    throw new InvalidOperationException("Erro ao carregar pedidos.");
                }

                // Para exibir o endereço sem falhas em acentos.
                foreach (var item in pedidos)
                {
                    item.Endereco = WebUtility.HtmlDecode(item.Endereco);

                }
                
                // Retorna a view com os pedidos
                return View(pedidos);
            }
            catch (Exception ex)
            {
                // Loga qualquer outra exceção genérica
                _logger.LogError(ex, "Erro inesperado ao buscar pedidos.");
                TempData["Alerta"] = "Ocorreu um erro ao tentar carregar os pedidos. Tente novamente mais tarde.";
                return RedirectToAction("Index", "Home");
            }
        }

        // Método para atualizar o status do pedido, ADMIN.
        [PaginaAdmin]
        [HttpPost]
        public async Task<IActionResult> AtualizarStatusDoPedido(int pedidoId, string status)
        {
            try
            {
                // Converte a string recebida para o enum PedidoEnum
                if (!Enum.TryParse(status, out PedidoEnum pedidoStatus))
                {
                    throw new InvalidOperationException("Status inválido.");
                }

                // Busca o pedido pelo ID
                var pedido = await _pedidoRepositorio.BuscarPedidoPorIdAsync(pedidoId);
                if (pedido == null)
                {
                    throw new InvalidOperationException("Pedido não encontrado.");
                }
                // Atualiza o status do pedido
                pedido.StatusDoPedido = pedidoStatus;

                await _pedidoRepositorio.AtualizarPedidoAsync(pedido);

                TempData["Sucesso"] = "Status do pedido atualizado com sucesso.";
                return RedirectToAction("PedidosEPagamentos");
            }
            catch (Exception ex)
            {
                // Loga qualquer outra exceção genérica
                _logger.LogError(ex, "Erro inesperado ao atualizar status do pedido.");
                TempData["Alerta"] = "Ocorreu um erro ao tentar atualizar o status do pedido. Tente novamente mais tarde.";
                return RedirectToAction("PedidosEPagamentos");
            }
        }

    }
}
