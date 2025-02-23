using DigitalStore.Helper;
using DigitalStore.Models;
using DigitalStore.Models.ViewModels;
using DigitalStore.Repositorio;
using DigitalStore.Repositorio.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Stripe;

namespace DigitalStore.Controllers
{
    public class CarrinhoController : Controller
    {
        // Declarando as dependências do repositório
        private readonly ICarrinhoRepositorio _carrinhoRepositorio;

        private readonly IUsuarioRepositorio _usuarioRepositorio;
        private readonly IProdutoRepositorio _produtoRepositorio;
        private readonly ISessao _sessao;
        private readonly IEnderecoRepositorio _enderecoRepositorio;

        // Construtor para injeção de dependência
        public CarrinhoController(ICarrinhoRepositorio carrinhoRepositorio,
                                    IUsuarioRepositorio usuarioRepositorio,
                                    IProdutoRepositorio produtoRepositorio, ISessao sessao, IEnderecoRepositorio enderecoRepositorio)
        {
            _carrinhoRepositorio = carrinhoRepositorio;
            _usuarioRepositorio = usuarioRepositorio;
            _produtoRepositorio = produtoRepositorio;
            _sessao = sessao;
            _enderecoRepositorio = enderecoRepositorio;
        }

        public async Task<IActionResult> Carrinho(int id)
        {
            List<CarrinhoModel> carrinho = await _carrinhoRepositorio.BuscarCarrinhoDoUsuarioAsync(id);

            if (carrinho == null)
            {
                return RedirectToAction("Index", "Home");
            }
            var endereco = new EnderecoModel
            {
                UsuarioId = id,
                EnderecoCompleto = string.Empty // Definindo o membro requerido
            };

            var viewModel = new CarrinhoViewModel
            {
                Endereco = endereco,
                Enderecos = await _enderecoRepositorio.BuscarTodosOsEnderecosDoUsuarioAsync(id),
                Carrinho = carrinho
            };

            return View(viewModel);
        }

        public class CarrinhoRequest
        {
            public int ProdutoId { get; set; }
        }

        // Método para adicionar um produto ao carrinho
        [HttpPost]
        public async Task<IActionResult> AddOuRemoverCarrinho([FromBody] CarrinhoRequest request)
        {
            try
            {
                var usuario = _sessao.BuscarSessaoDoUsuario();
                var produto = await _produtoRepositorio.BuscarProdutoPorIdAsync(request.ProdutoId);

                if (usuario == null || produto == null)
                {
                    return Json(new { success = false });
                }

                await _carrinhoRepositorio.AddOuRemoverCarrinhoAsync(request.ProdutoId, usuario.UsuarioId);

                var novoCarrinho = await _carrinhoRepositorio.BuscarProdutoExistenteNoCarrinhoAsync(request.ProdutoId, usuario.UsuarioId);

                return Json(new { success = true, carrinhoAtivo = novoCarrinho != null });
            }
            catch (Exception ex)
            {
                // Em caso de erro, loga a exceção e retorna uma página de erro
                Console.WriteLine(ex.Message);
                return StatusCode(500, "Erro ao adicionar produto ao carrinho.");
            }
        }

        [HttpPost]
        public async Task<IActionResult> AtualizarQuantidade(int ProdutoId, int usuarioId, string acao)
        {
            try
            {
                var produtoNoCarrinho = await _carrinhoRepositorio.BuscarProdutoExistenteNoCarrinhoAsync(ProdutoId, usuarioId);
                if (produtoNoCarrinho == null)
                {
                   TempData["MensagemErro"] = "Produto não encontrado no carrinho.";
                }
                if (acao == "aumentar")
                {
                    produtoNoCarrinho.Quantidade += 1;
                }
                else if (acao == "diminuir" && produtoNoCarrinho.Quantidade > 1)
                {
                    produtoNoCarrinho.Quantidade -= 1;
                }

                await _carrinhoRepositorio.AtualizarQuantidadeAsync(ProdutoId, usuarioId, produtoNoCarrinho.Quantidade);
                return RedirectToAction("Carrinho", new { id = usuarioId });
            }
            catch (Exception ex)
            {
                // Em caso de erro, loga a exceção e retorna uma página de erro
                Console.WriteLine(ex.Message);
                return StatusCode(500, "Erro ao adicionar produto ao carrinho.");
            }

        }
    }
}