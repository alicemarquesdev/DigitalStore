using DigitalStore.Helper;
using DigitalStore.Models;
using DigitalStore.Models.ViewModels;
using DigitalStore.Repositorio;
using DigitalStore.Repositorio.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Stripe;

namespace DigitalStore.Controllers
{
    public class PagamentoController : Controller
    {
        private readonly StripeSettings _stripeSettings;
        private readonly IEnderecoRepositorio _enderecoRepositorio;
        private readonly IUsuarioRepositorio _usuarioRepositorio;
        private readonly ICarrinhoRepositorio _carrinhoRepositorio;
        private readonly ISessao _sessao;
        private readonly StripeService _stripeService;
        private readonly IPagamentoRepositorio _pagamentoRepositorio;
        private readonly IPedidoRepositorio _pedidoRepositorio;
        private readonly IItensDoPedidoRepositorio _itensDoPedidoRepositorio;

        public PagamentoController(IOptions<StripeSettings> stripeSettings,
            IEnderecoRepositorio enderecoRepositorio,
            IUsuarioRepositorio usuarioRepositorio,
            ICarrinhoRepositorio carrinhoRepositorio,
            ISessao sessao, StripeService stripeService,
            IPagamentoRepositorio pagamentoRepositorio,
            IPedidoRepositorio pedidoRepositorio,
            IItensDoPedidoRepositorio itensDoPedidoRepositorio)
        {
            _stripeSettings = stripeSettings.Value;
            Console.WriteLine($"Chave Stripe carregada no Controller: {_stripeSettings.SecretKey}");

            StripeConfiguration.ApiKey = "sk_test_51Qcwp7JdK4WCtqrXZ2AM7RBLGO8wLgukfKMdWOzrCjKhjVm6MzLgcFPIZe2YY0KMBq2ExLY6bNZTbUuIXyOTRZ0t00r1zcEwb3"; // Definir a chave secreta do Stripe
            _enderecoRepositorio = enderecoRepositorio;
            _usuarioRepositorio = usuarioRepositorio;
            _carrinhoRepositorio = carrinhoRepositorio;
            _sessao = sessao;
            _stripeService = stripeService;
            _pagamentoRepositorio = pagamentoRepositorio;
            _pedidoRepositorio = pedidoRepositorio;
            _itensDoPedidoRepositorio = itensDoPedidoRepositorio;
        }

        // Ação que carrega a página de pagamento
        public async Task<IActionResult> Pagamento(int id)
        {
            var usuario = _sessao.BuscarSessaoDoUsuario();

            if (usuario == null)
            {
                Console.WriteLine("Usuario não encontrado");
                return RedirectToAction("Carrinho", "Carrinho", new { id = usuario.UsuarioId });
            }

            var endereco = await _enderecoRepositorio.BuscarEnderecoPorIdAsync(id);
            if (endereco == null)
            {
                Console.WriteLine("Endereço não encontrado");
                return RedirectToAction("Carrinho", "Carrinho", new { id = usuario.UsuarioId });
            }

            var pagamento = new PagamentoModel();

            var carrinho = await _carrinhoRepositorio.BuscarCarrinhoDoUsuarioAsync(usuario.UsuarioId);
            if (carrinho == null)
            {
                Console.WriteLine("Carrinho não encontrado");
                return RedirectToAction("Carrinho", "Carrinho", new { id = usuario.UsuarioId });
            }

            var frete = await _enderecoRepositorio.ObterEstadoDoEnderecoAsync(endereco.EnderecoCompleto);

            var viewModel = new PagamentoViewModel
            {
                CarrinhoTotal = carrinho.Sum(x => x.Produto.Preco),
                Pagamento = pagamento,
                Endereco = endereco,
                Frete = _enderecoRepositorio.CalcularFretePorRegiao(frete),
                UsuarioId = usuario.UsuarioId,
                StripeKey = "pk_test_51Qcwp7JdK4WCtqrXUbyXtGNl59DXGXKG7NEQeZte1c8tM1xam9IXgYPmE5DlnzHRWHpjMbL7uIyzfvsGuEhnGPbb00WvjgYFSw",
                Valor = carrinho.Sum(x => x.Produto.Preco) + _enderecoRepositorio.CalcularFretePorRegiao(frete),  // Valor total do pagamento
                Moeda = "brl",  // Moeda que você está usando, por exemplo "brl" para real
                Descricao = "Pagamento de pedido na Digital Store",  // Descrição do pagamento
                Token = "token_do_frontend"  // O token que será gerado no frontend pelo Stripe
            };

            return View(viewModel);
        }

        // Ação que mostra a página de sucesso
        public IActionResult PagamentoSucesso()
        {
            return View();
        }

        // Ação que mostra a página de cancelamento
        public IActionResult Cancelamento()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ProcessarPagamento([FromBody] StripeModel stripeModel)
        {
            if (stripeModel == null)
            {
                return Json(new { success = false, message = "Modelo de pagamento está nulo." });
            }

            // Verifica se o token foi recebido corretamente
            if (string.IsNullOrEmpty(stripeModel.Token))
            {
                return Json(new { success = false, message = "Token inválido" });
            }

            try
            {
                // Cria as opções de cobrança com o valor e o token
                var chargeOptions = new ChargeCreateOptions
                {
                    Amount = (long)(stripeModel.Valor * 100),  // Multiplica o valor por 100 para obter centavos
                    Currency = "BRL",  // Moeda
                    Source = stripeModel.Token // Token gerado pelo frontend
                };

                var chargeService = new ChargeService();
                var charge = await chargeService.CreateAsync(chargeOptions);

                if (charge.Status == "succeeded")
                {
                    // Cria o pedido
                    var pedido = new PedidoModel
                    {
                        UsuarioId = stripeModel.UsuarioId,
                        EnderecoId = stripeModel.EnderecoId
                    };

                    // Adiciona o pedido ao banco de dados
                    await _pedidoRepositorio.AddPedidoAsync(pedido);


                    var carrinho = await _carrinhoRepositorio.BuscarCarrinhoDoUsuarioAsync(stripeModel.UsuarioId);
                    foreach (var produto in carrinho)
                    {
                        var itemDoPedido = new ItensDoPedidoModel
                        {
                            ProdutoId = produto.ProdutoId,
                            PedidoId = pedido.PedidoId,
                            QuantidadeItem = produto.Quantidade,
                            PrecoUnidadeItem = produto.Produto.Preco
                        };

                        await _itensDoPedidoRepositorio.AddItemAsync(itemDoPedido);
                    }


                    // Cria o pagamento com o ID do pedido que acabou de ser criado
                    var pagamento = new PagamentoModel
                    {
                        PedidoId = pedido.PedidoId, // Associar o pedido criado
                        MetodoPagamento = Enums.MetodoPagamentoEnum.Cartao,
                        StatusPagamento = Enums.StatusPagamentoEnum.Pago,
                        TokenStripe = stripeModel.Token,
                        Valor = stripeModel.Valor,
                    };

                    // Adiciona o pagamento ao banco de dados
                    await _pagamentoRepositorio.AddPagamento(pagamento);

                    // Atualiza o pedido com o ID do pagamento
                    pedido.PagamentoId = pagamento.PagamentoId;
                    await _pedidoRepositorio.AtualizarPedidoAsync(pedido);

                    return Json(new { success = true });
                }
                else
                {
                    return Json(new { success = false, message = "Falha no pagamento" });
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }


    }
}
