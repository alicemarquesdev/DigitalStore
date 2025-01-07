using DigitalStore.Models;
using DigitalStore.Repositorio.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace DigitalStore.Controllers
{
    public class CarrinhoController : Controller
    {
        // Declarando as dependências do repositório
        private readonly ICarrinhoRepositorio _carrinhoRepositorio;

        private readonly IUsuarioRepositorio _usuarioRepositorio;
        private readonly IProdutoRepositorio _produtoRepositorio;

        // Construtor para injeção de dependência
        public CarrinhoController(ICarrinhoRepositorio carrinhoRepositorio,
                                    IUsuarioRepositorio usuarioRepositorio,
                                    IProdutoRepositorio produtoRepositorio)
        {
            _carrinhoRepositorio = carrinhoRepositorio;
            _usuarioRepositorio = usuarioRepositorio;
            _produtoRepositorio = produtoRepositorio;
        }

        public async Task<IActionResult> Carrinho(int id)
        {
            List<CarrinhoModel> produtos = await _carrinhoRepositorio.BuscarCarrinhoDoUsuarioAsync(id);

            ViewBag.TotalCarrinho = produtos.Sum(x => x.Produto?.Preco ?? 0);
            ViewBag.UsuarioId = id;

            return View(produtos);
        }

        // Método para adicionar um produto ao carrinho
        [HttpPost]
        public async Task<IActionResult> AddCarrinho(int produtoId, int usuarioId)
        {
            try
            {
                var urlAnterior = Request.Headers["Referer"].ToString();

                // Chama o repositório para adicionar o produto ao carrinho
                await _carrinhoRepositorio.AddAoCarrinhoAsync(produtoId, usuarioId);

                // Redireciona para a página principal do carrinho
                return Redirect(urlAnterior);
            }
            catch (Exception ex)
            {
                // Em caso de erro, loga a exceção e retorna uma página de erro
                Console.WriteLine(ex.Message);
                return StatusCode(500, "Erro ao adicionar produto ao carrinho.");
            }
        }

        // Método para remover um produto do carrinho
        [HttpPost]
        public async Task<IActionResult> RemoverCarrinho(int produtoId, int usuarioId)
        {
            try
            {
                var urlAnterior = Request.Headers["Referer"].ToString();

                // Chama o repositório para remover o produto do carrinho
                await _carrinhoRepositorio.RemoverDoCarrinhoAsync(produtoId, usuarioId);

                // Redireciona para a página principal do carrinho
                return Redirect(urlAnterior);
            }
            catch (Exception ex)
            {
                // Em caso de erro, loga a exceção e retorna uma página de erro
                Console.WriteLine(ex.Message);
                return StatusCode(500, "Erro ao remover produto do carrinho.");
            }
        }

        // Método para adicionar ou remover produto do carrinho com base no estado atual
        [HttpPost]
        public async Task<IActionResult> BotaoCarrinho(int produtoId, int usuarioId)
        {
            try
            {
                var urlAnterior = Request.Headers["Referer"].ToString();

                // Busca o usuário e o produto
                var usuario = await _usuarioRepositorio.BuscarUsuarioPorIdAsync(usuarioId);
                var produto = await _produtoRepositorio.BuscarProdutoPorIdAsync(produtoId);

                // Verifica se o usuário e o produto existem
                if (usuario == null || produto == null)
                {
                    // Se o produto ou usuário não existir, redireciona para uma página de erro ou retorna algo apropriado
                    return NotFound(); // Ou redireciona para uma página de erro
                }

                // Verifica se o produto já está no carrinho
                var produtoExistente = await _carrinhoRepositorio.BuscarProdutoExistenteNoCarrinhoAsync(produtoId, usuarioId);

                if (produtoExistente != null)
                {
                    // Se o produto já existe no carrinho, remove-o
                    await RemoverCarrinho(produtoId, usuarioId);
                }
                else
                {
                    // Caso contrário, adiciona o produto ao carrinho
                    await AddCarrinho(produtoId, usuarioId);
                }

                // Após a operação, redireciona para a página anterior (de onde veio a requisição)
                return Redirect(urlAnterior);
            }
            catch (Exception ex)
            {
                // Em caso de erro, loga a exceção e retorna uma página de erro
                Console.WriteLine(ex.Message);
                return StatusCode(500, "Erro ao processar operação no carrinho.");
            }
        }
    }
}