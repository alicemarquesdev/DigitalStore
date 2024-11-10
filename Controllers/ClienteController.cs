using DigitalStore.Helper;
using DigitalStore.Models;
using DigitalStore.Repositorio;
using Microsoft.AspNetCore.Mvc;

namespace DigitalStore.Controllers
{
    public class ClienteController : Controller
    {
        private readonly IUsuarioRepositorio _usuarioRepositorio;
        private readonly ISessao _sessao;

        public ClienteController(IUsuarioRepositorio usuarioRepositorio, ISessao sessao)
        {
            _usuarioRepositorio = usuarioRepositorio;
            _sessao = sessao;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult ApagarConta()
        {
            return View();
        }

        public async Task<IActionResult> EditarDados(int id)
        {
            var usuario = await _usuarioRepositorio.BuscarUsuarioPorIdAsync(id);
            return View(usuario);
        }

        public IActionResult Sair()
        {
            _sessao.RemoverSessaoDoUsuario();
            return RedirectToAction("Home", "Index");
        }

        [HttpPost]
        public async Task<IActionResult> Editar(UsuarioSemSenhaModel usuario)
        {
            try
            {
                if (usuario == null) throw new Exception("Houve um erro ao tentar atualizar os dados do usuário");

                if (ModelState.IsValid)
                {
                    await _usuarioRepositorio.AtualizarUsuarioAsync(usuario);
                    TempData["MensagemSucesso"] = "O usuário foi criado com sucesso!";
                    return RedirectToAction("Index", "Home");
                }

                TempData["MensagemErro"] = "Houve um problema ao atualizar os dados, tente novamente.";
                return View(usuario);
            }
            catch (Exception)
            {
                throw new Exception();
            }
        }
    }
}