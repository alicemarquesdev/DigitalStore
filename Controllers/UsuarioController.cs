using DigitalStore.Models;
using DigitalStore.Repositorio;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DigitalStore.Controllers
{
    public class UsuarioController : Controller
    {
        private readonly IUsuarioRepositorio _usuarioRepositorio;

        public UsuarioController(IUsuarioRepositorio usuarioRepositorio)
        {
            _usuarioRepositorio = usuarioRepositorio;
        }

        public async Task<IActionResult> ApagarUsuario(int id)
        {
            UsuarioModel usuario = await _usuarioRepositorio.BuscarUsuarioPorIdAsync(id);

            ViewBag.UsuarioNome = usuario.Nome;
            return View(usuario);
        }

        public async Task<IActionResult> ApagarUsuarioConfirmacao(int id)
        {
            await _usuarioRepositorio.RemoverUsuarioAsync(id);
            return RedirectToAction("Index", "Home");
        }

        public IActionResult CriarUsuario()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CriarUsuario(UsuarioModel usuario)
        {
            if (ModelState.IsValid)
            {
                await _usuarioRepositorio.AddUsuarioAsync(usuario);
                TempData["MensagemSucesso"] = "O usuário foi criado com sucesso!";
                return RedirectToAction("Index", "Login");
            }

            return View(usuario);
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
    }
}