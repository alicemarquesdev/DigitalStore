using DigitalStore.Enum;
using DigitalStore.Helper;
using DigitalStore.Models;
using DigitalStore.Repositorio;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace DigitalStore.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IProdutoRepositorio _produtoRepositorio;
        private readonly ISessao _sessao;
        private readonly IUsuarioRepositorio _usuarioRepositorio;

        public HomeController(ILogger<HomeController> logger, IProdutoRepositorio produtoRepositorio, ISessao sessao, IUsuarioRepositorio usuarioRepositorio)
        {
            _logger = logger;
            _produtoRepositorio = produtoRepositorio;
            _sessao = sessao;
            _usuarioRepositorio = usuarioRepositorio;
        }

        public IActionResult CriarConta()
        {
            return View();
        }

        public IActionResult Login()
        {
            if (_sessao.BuscarSessaoDoUsuario() != null) RedirectToAction("Index", "Home");
            return View();
        }

        public async Task<IActionResult> RedefinirSenha(int id)
        {
            await _usuarioRepositorio.BuscarUsuarioPorIdAsync(id);
            return View("RedefinirSenha");
        }

        public async Task<IActionResult> Index()
        {
            var usuario = _sessao.BuscarSessaoDoUsuario();

            if (usuario == null)
            {
                ViewData["Layout"] = "_LayoutDeslogado";
            }
            else if (usuario.Perfil == Enum.PerfilEnum.Admin)
            {
                ViewData["Layout"] = "_LayoutAdmin";
            }
            else
            {
                ViewData["Layout"] = "_Layout";
            }

            List<ProdutoModel> produtos = await _produtoRepositorio.BuscarTodosProdutosAsync();

            return View(produtos);
        }

        public async Task<IActionResult> Categoria(ProdutoModel categoriaProdutos)
        {
            List<ProdutoModel> produtosPorIdioma = await _produtoRepositorio.BuscarProdutoPorCategoriaAsync(categoriaProdutos);
            ViewBag.CategoriaSelecionada = categoriaProdutos.Categoria;
            return View(produtosPorIdioma);
        }

        public IActionResult Suporte()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddUsuario(UsuarioModel usuario)
        {
            if (ModelState.IsValid)
            {
                await _usuarioRepositorio.AddUsuarioAsync(usuario);
                TempData["MensagemSucesso"] = "O usuário foi criado com sucesso!";
                return RedirectToAction("Login", "Usuario");
            }

            return View(usuario);
        }

        [HttpPost]
        public async Task<IActionResult> Entrar(LoginModel loginModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var usuario = await _usuarioRepositorio.BuscarUsuarioExistenteAsync(loginModel.Email);

                    if (usuario != null)
                    {
                        if (usuario.SenhaValida(loginModel.Senha))
                        {
                            if (usuario.Perfil == PerfilEnum.Admin)
                            {
                                _sessao.CriarSessaoDoUsuario(usuario);
                                return RedirectToAction("Index", "Admin");
                            }

                            _sessao.CriarSessaoDoUsuario(usuario);
                            return RedirectToAction("Index", "Home");
                        }

                        TempData["MensagemErro"] = "Senha do usuário é inválida, tente novamente.";
                    }

                    TempData["MensagemErro"] = "Usuário e/ou senha inválido(s). Por favor, tente novamente.";
                }

                return View("Login");
            }
            catch (Exception)
            {
                TempData["MensagemErro"] = "Usuário e/ou senha inválido(s). Por favor, tente novamente.";
                return RedirectToAction("Login");
            }
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}