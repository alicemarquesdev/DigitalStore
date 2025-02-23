using DigitalStore.Helper;
using DigitalStore.Models;
using DigitalStore.Repositorio.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace DigitalStore.Controllers
{
    public class PedidoController : Controller
    {
        private readonly IItensDoPedidoRepositorio _itensDoPedidoRepositorio;
        private readonly IPedidoRepositorio _pedidoRepositorio;
        private readonly IEnderecoRepositorio _enderecoRepositorio;
        private readonly IUsuarioRepositorio _usuarioRepositorio;
        private readonly ICarrinhoRepositorio _carrinhoRepositorio;
        private readonly ISessao _sessao;

        public PedidoController(IItensDoPedidoRepositorio itensDoPedidoRepositorio, IPedidoRepositorio pedidoRepositorio,
                                 IUsuarioRepositorio usuarioRepositorio,
                      ICarrinhoRepositorio carrinhoRepositorio,
                      ISessao sessao, IEnderecoRepositorio enderecoRepositorio)
        {
            _enderecoRepositorio = enderecoRepositorio;
            _itensDoPedidoRepositorio = itensDoPedidoRepositorio;
            _pedidoRepositorio = pedidoRepositorio;
            _usuarioRepositorio = usuarioRepositorio;
            _carrinhoRepositorio = carrinhoRepositorio;
            _sessao = sessao;
        }

        public async Task<IActionResult> MeusPedidos()
        {
            UsuarioModel usuario = _sessao.BuscarSessaoDoUsuario();

            List<PedidoModel> pedidos = await _pedidoRepositorio.BuscarTodosOsPedidosDoUsuarioAsync(usuario.UsuarioId);
            return View(pedidos);
        }

    }
}