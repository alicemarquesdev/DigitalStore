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

        // Exibe a página de alteração de senha
        public async Task<IActionResult> AlterarSenha(int id)
        {
            var usuario = await _usuarioRepositorio.BuscarUsuarioPorIdAsync(id);
            if (usuario == null)
            {
                TempData["MensagemErro"] = "Houve um erro, por favor tente novamente";
            }

            var alterarSenhaModel = new AlterarSenhaModel()
            {
                Id = usuario.UsuarioId,
                SenhaAtual = string.Empty,
                NovaSenha = string.Empty,
                ConfirmarNovaSenha = string.Empty
            };

            return View(alterarSenhaModel);
        }

        // Exibe a página da conta do usuário
        public IActionResult MinhaConta()
        {
            var usuarioSessao = _sessao.BuscarSessaoDoUsuario();

            return View(usuarioSessao);
        }

        // Método para editar os dados do usuário
        [HttpPost]
        public async Task<IActionResult> EditarUsuario(UsuarioSemSenhaModel usuarioSemSenha)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var usuarioDb = await _usuarioRepositorio.BuscarUsuarioPorIdAsync(usuarioSemSenha.Id);

                    // Criação de um novo modelo de usuário comos dados fornecidos
                    var usuario = new UsuarioModel()
                    {
                        UsuarioId = usuarioSemSenha.Id,
                        Nome = usuarioSemSenha.Nome,
                        Email = usuarioSemSenha.Email,
                        Perfil = usuarioDb.Perfil,
                        Senha = usuarioDb.Senha
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

        // Método para confirmação de apagar o usuário
        [HttpPost]
        public async Task<IActionResult> RemoverUsuario(int id)
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
    }
}