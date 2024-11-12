using DigitalStore.Helper;
using DigitalStore.Models;
using DigitalStore.Repositorio;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace DigitalStore.Controllers
{
    public class HomeController : Controller
    {
        private readonly IProdutoRepositorio _produtoRepositorio;
        private readonly ISessao _sessao;
        private readonly IUsuarioRepositorio _usuarioRepositorio;

        public HomeController(IProdutoRepositorio produtoRepositorio,
                              IUsuarioRepositorio usuarioRepositorio,
                              ISessao sessao)
        {
            _produtoRepositorio = produtoRepositorio;
            _usuarioRepositorio = usuarioRepositorio;
            _sessao = sessao;
        }

        public async Task<IActionResult> Categoria(ProdutoModel categoriaProdutos)
        {
            List<ProdutoModel> produtosPorIdioma = await _produtoRepositorio.BuscarProdutoPorCategoriaAsync(categoriaProdutos);
            ViewBag.CategoriaSelecionada = categoriaProdutos.Categoria;

            var sessao = _sessao.BuscarSessaoDoUsuario();

            if (sessao != null)
            {
                UsuarioModel usuario = await _usuarioRepositorio.BuscarUsuarioPorIdAsync(sessao.UsuarioId);

                ViewBag.UsuarioId = usuario.UsuarioId;
            }

            ViewBag.Sessao = sessao;

            return View(produtosPorIdioma);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
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

        public IActionResult Suporte()
        {
            return View();
        }
    }
}