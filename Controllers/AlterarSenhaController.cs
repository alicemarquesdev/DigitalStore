using DigitalStore.Helper;
using DigitalStore.Models;
using DigitalStore.Repositorio;
using Microsoft.AspNetCore.Mvc;

namespace DigitalStore.Controllers
{
    public class AlterarSenhaController : Controller
    {
        private readonly ISessao _sessao;
        private readonly IUsuarioRepositorio _usuarioRepositorio;

        public AlterarSenhaController(ISessao sessao, IUsuarioRepositorio usuarioRepositorio)
        {
            _usuarioRepositorio = usuarioRepositorio;
            _sessao = sessao;
        }

        [HttpPost]
        public async Task<IActionResult> Alterar(AlterarSenhaModel alterarSenhaModel)
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

        public IActionResult Index()
        {
            return View();
        }
    }
}