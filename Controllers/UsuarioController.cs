using DigitalStore.Helper;
using DigitalStore.Models;
using DigitalStore.Repositorio.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace DigitalStore.Controllers
{
    public class UsuarioController : Controller
    {
        private readonly IUsuarioRepositorio _usuarioRepositorio;
        private readonly ISessao _sessao;

        public UsuarioController(IUsuarioRepositorio usuarioRepositorio,
                                    ISessao sessao)
        {
            _usuarioRepositorio = usuarioRepositorio;
            _sessao = sessao;
        }

        public async Task<IActionResult> ApagarUsuario(int id)
        {
            UsuarioModel usuario = await _usuarioRepositorio.BuscarUsuarioPorIdAsync(id);

            ViewBag.UsuarioNome = usuario.Nome;
            return View(usuario);
        }

        [HttpPost]
        public async Task<IActionResult> ApagarUsuarioConfirmacao(int id)
        {
            await _usuarioRepositorio.RemoverUsuarioAsync(id);
            return RedirectToAction("Index", "Home");
        }

        public async Task<IActionResult> EditarUsuario(int id)
        {
            var usuario = await _usuarioRepositorio.BuscarUsuarioPorIdAsync(id);
            if (usuario == null)
            {
                TempData["MensagemErro"] = "Houve um erro, por favor tente novamente";
            }

            return View(usuario);
        }

        [HttpPost]
        public async Task<IActionResult> EditarUsuario(UsuarioSemSenhaModel usuarioSemSenha)
        {
            try
            {
                UsuarioModel usuario = null;

                if (ModelState.IsValid)
                {
                    usuario = new UsuarioModel()
                    {
                        UsuarioId = usuarioSemSenha.Id,
                        Nome = usuarioSemSenha.Nome,
                        Email = usuarioSemSenha.Email,
                    };
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

        [HttpPost]
        public async Task<IActionResult> AlterarSenha(AlterarSenhaModel alterarSenhaModel)
        {
            try
            {
                UsuarioModel usuarioLogado = _sessao.BuscarSessaoDoUsuario();
                alterarSenhaModel.Id = usuarioLogado.UsuarioId;

                if (ModelState.IsValid)
                {
                    await _usuarioRepositorio.AlterarSenhaAsync(alterarSenhaModel);
                    TempData["MensagemSucesso"] = "Senha alterada com sucesso!";
                    return View("Index", alterarSenhaModel);
                }

                return View("Index", alterarSenhaModel);
            }
            catch (Exception erro)
            {
                TempData["MensagemErro"] = $"Ops, não conseguimos alterar sua senha, tente novamante, detalhe do erro: {erro.Message}";
                return View("Index", alterarSenhaModel);
            }
        }

        public IActionResult AlterarSenha()
        {
            return View();
        }

        public IActionResult MinhaConta()
        {
            return View();
        }
    }
}