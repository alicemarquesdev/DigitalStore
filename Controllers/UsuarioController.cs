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

        // Construtor para injeção de dependência
        public UsuarioController(IUsuarioRepositorio usuarioRepositorio, ISessao sessao)
        {
            _usuarioRepositorio = usuarioRepositorio;
            _sessao = sessao;
        }

        // Exibe a página de apagar usuário com confirmação
        public async Task<IActionResult> ApagarUsuario(int id)
        {
            UsuarioModel usuario = await
                _usuarioRepositorio.BuscarUsuarioPorIdAsync(id);

            ViewBag.UsuarioNome = usuario.Nome;
            return View(usuario);
        }

        // Exibe a página para editar os dados do usuário
        public async Task<IActionResult> EditarUsuario(int id)
        {
            var usuario = await _usuarioRepositorio.BuscarUsuarioPorIdAsync(id);
            if (usuario == null)
            {
                TempData["MensagemErro"] = "Houve um erro, por favor tente novamente";
            }

            return View(usuario);
        }

        // Exibe a página de alteração de senha
        public IActionResult AlterarSenha()
        {
            return View();
        }

        // Exibe a página da conta do usuário
        public IActionResult MinhaConta()
        {
            return View();
        }

        // Método para confirmação de apagar o usuário
        [HttpPost]
        public async Task<IActionResult> ApagarUsuarioConfirmacao(int id)
        {
            try
            {
                await _usuarioRepositorio.RemoverUsuarioAsync(id);
                return RedirectToAction("Index", "Home");
            }
            catch (Exception erro)
            {
                TempData["MensagemErro"] = $"Ops, não conseguimos apagar o usuário, tente novamente, detalhe do erro: {erro.Message}";
                return RedirectToAction("ApagarUsuario", new { id });
            }
        }

        // Método para editar os dados do usuário
        [HttpPost]
        public async Task<IActionResult> EditarUsuario(UsuarioSemSenhaModel usuarioSemSenha)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    // Criação de um novo modelo de usuário com os dados fornecidos
                    UsuarioModel usuario = new UsuarioModel()
                    {
                        UsuarioId = usuarioSemSenha.Id,
                        Nome = usuarioSemSenha.Nome,
                        Email = usuarioSemSenha.Email,
                    };

                    await _usuarioRepositorio.AtualizarUsuarioAsync(usuario);
                    TempData["MensagemSucesso"] = "Os dados do usuário foram atualizados com sucesso!";
                    return RedirectToAction("Index", "Home");
                }

                TempData["MensagemErro"] = "Houve um problema ao atualizar os dados, tente novamente.";
                return View(usuarioSemSenha);
            }
            catch (Exception erro)
            {
                TempData["MensagemErro"] = $"Erro ao tentar atualizar o usuário: {erro.Message}";
                return View(usuarioSemSenha);
            }
        }

        // Método para alterar a senha do usuário
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
                    return RedirectToAction("Index", "Home");
                }

                return View("AlterarSenha", alterarSenhaModel);
            }
            catch (Exception erro)
            {
                TempData["MensagemErro"] = $"Ops, não conseguimos alterar sua senha, tente novamente. Detalhe do erro: {erro.Message}";
                return View("AlterarSenha", alterarSenhaModel);
            }
        }
    }
}