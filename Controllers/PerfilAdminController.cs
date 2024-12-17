using DigitalStore.Helper;
using DigitalStore.Models;
using DigitalStore.Repositorio.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace DigitalStore.Controllers
{
    public class PerfilAdminController : Controller
    {
        private readonly IProdutoRepositorio _produtoRepositorio;
        private readonly ISessao _sessao;
        private readonly IUsuarioRepositorio _usuarioRepositorio;

        public PerfilAdminController(IProdutoRepositorio produtoRepositorio,
                              IUsuarioRepositorio usuarioRepositorio,
                              ISessao sessao)
        {
            _produtoRepositorio = produtoRepositorio;
            _usuarioRepositorio = usuarioRepositorio;
            _sessao = sessao;
        }

        public async Task<IActionResult> Index()
        {
            List<ProdutoModel> produtos = await _produtoRepositorio.BuscarTodosProdutosAsync();

            var sessao = _sessao.BuscarSessaoDoUsuario();

            if (sessao != null)
            {
                UsuarioModel usuario = await _usuarioRepositorio.BuscarUsuarioPorIdAsync(sessao.UsuarioId);

                ViewBag.UsuarioId = usuario.UsuarioId;
            }

            ViewBag.Sessao = sessao;

            return View(produtos);
        }

        public IActionResult AddProduto()
        {
            return View();
        }

        public async Task<IActionResult> AtualizarProduto(int id)
        {
            ProdutoModel produto = await _produtoRepositorio.BuscarProdutoPorIdAsync(id);

            ViewBag.Categorias = await _produtoRepositorio.BuscarCategoriasAsync();

            return View(produto);
        }

        public async Task<IActionResult> GerenciamentoProdutos()
        {
            var produtos = await _produtoRepositorio.BuscarTodosProdutosAsync();
            return View(produtos);
        }

        public async Task<IActionResult> Personalizacao()
        {
            var produtos = await _produtoRepositorio.BuscarTodosProdutosAsync();
            return View(produtos);
        }
    }
}