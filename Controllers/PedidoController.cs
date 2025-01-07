using DigitalStore.Enums;
using DigitalStore.Helper;
using DigitalStore.Models;
using DigitalStore.Repositorio;
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

        public async Task<IActionResult> MeusPedidos(int usuarioId)
        {
            List<PedidoModel> pedidos = await _pedidoRepositorio.BuscarTodosOsPedidosDoUsuarioAsync(usuarioId);
            return View(pedidos);
        }

        [HttpPost]
        public async Task<IActionResult> CriarPedido(EnderecoModel endereco)
        {
            try
            {
                UsuarioModel usuario = _sessao.BuscarSessaoDoUsuario();

                if (usuario == null)
                {
                    TempData["MensagemErro"] = "Usuario não encontrado";
                    return RedirectToAction("Endereco", "Endereco");
                }

                var carrinho = await _carrinhoRepositorio.BuscarCarrinhoDoUsuarioAsync(usuario.UsuarioId);

                if (carrinho == null || !carrinho.Any())
                {
                    TempData["MensagemErro"] = "Carrinho vazio.";
                    return RedirectToAction("Carrinho", "Carrinho");
                }

                if (!ModelState.IsValid)
                {
                    TempData["MensagemErro"] = "Houve um erro ao adicionar o endereço!";
                    return RedirectToAction("Endereco", "Endereco");
                }

                var novoEndereco = new EnderecoModel
                {
                    Rua = endereco.Rua,
                    Bairro = endereco.Bairro,
                    Cidade = endereco.Cidade,
                    Estado = endereco.Estado,
                    CEP = endereco.CEP
                };

                await _enderecoRepositorio.AddEnderecoAsync(endereco);

                var enderecoSalvo = await _enderecoRepositorio.BuscarEnderecoPorIdAsync(novoEndereco.EnderecoId);

                // Crie um novo pedido
                var novoPedido = new PedidoModel
                {
                    DataDoPedido = DateTime.Now,
                    ValorTotalDoPedido = carrinho.Sum(c => c.Produto.Preco),
                    StatusDoPedido = PedidoEnum.Pendente,
                    StatusPagamento = PagamentoEnum.Pendente,
                    UsuarioId = carrinho.First().UsuarioId, // Associa o pedido ao usuário
                    EnderecoId = enderecoSalvo.EnderecoId
                };

                await _pedidoRepositorio.AddPedidoAsync(novoPedido);

                // Adicione os itens ao pedido usando o método do repositório
                foreach (var item in carrinho)
                {
                    var novoItemPedido = new ItensDoPedidoModel
                    {
                        PedidoId = novoPedido.PedidoId,
                        ProdutoId = item.ProdutoId,
                        QuantidadeDeProdutos = 1, // Atualize conforme necessário
                        PrecoUnidadeProduto = item.Produto.Preco
                    };

                    await _itensDoPedidoRepositorio.AddItemAsync(novoItemPedido);
                }

                // Redirecione para uma página de sucesso ou resumo do pedido
                TempData["MensagemSucesso"] = "Endereço adicionado com sucesso!";
                return RedirectToAction("Pagamento", "Pagamento");
            }
            catch (Exception ex)
            {
                TempData["MensagemErro"] = $"Erro ao criar o pedido: {ex.Message}";
                return RedirectToAction("Endereco", "Endereco");
            }
        }
    }
}