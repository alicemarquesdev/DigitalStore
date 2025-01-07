using DigitalStore.Helper;
using DigitalStore.Models;
using DigitalStore.Repositorio.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace DigitalStore.Controllers
{
    public class LoginController : Controller
    {
        private readonly ISessao _sessao;
        private readonly IUsuarioRepositorio _usuarioRepositorio;
        private readonly IEmail _email;

        // Construtor com injeção de dependência
        public LoginController(ISessao sessao, IUsuarioRepositorio usuarioRepositorio, IEmail email)
        {
            _sessao = sessao;
            _usuarioRepositorio = usuarioRepositorio;
            _email = email;
        }

        public IActionResult CriarUsuario()
        {
            return View();
        }

        // Ação principal para exibir a tela de login
        public IActionResult Index()
        {
            if (_sessao.BuscarSessaoDoUsuario() != null)
                return RedirectToAction("Index", "Home"); // Redireciona se já houver uma sessão ativa
            return View();
        }

        // Exibe a tela de redefinir senha para o usuário específico
        public async Task<IActionResult> RedefinirSenha(int id)
        {
            var usuario = await _usuarioRepositorio.BuscarUsuarioPorIdAsync(id);
            return View(usuario);
        }

        // Método de logout que remove a sessão do usuário
        public IActionResult Sair()
        {
            _sessao.RemoverSessaoDoUsuario();
            return RedirectToAction("Index", "Login");
        }

        // Exibe a tela para criação de um novo usuário

        // Cria um novo usuário no sistema
        [HttpPost]
        public async Task<IActionResult> CriarUsuario(UsuarioModel usuario)
        {
            if (ModelState.IsValid)
            {
                await _usuarioRepositorio.AddUsuarioAsync(usuario);
                TempData["MensagemSucesso"] = "O usuário foi criado com sucesso!";
                return RedirectToAction("Index", "Login");
            }
            return View(usuario); // Retorna para a tela de criação caso os dados sejam inválidos
        }

        // Realiza o login do usuário
        [HttpPost]
        public async Task<IActionResult> Entrar(LoginModel loginModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var usuario = await _usuarioRepositorio.BuscarUsuarioExistenteAsync(loginModel.Email);

                    if (usuario != null && usuario.SenhaValida(loginModel.Senha))
                    {
                        _sessao.CriarSessaoDoUsuario(usuario);
                        return RedirectToAction("Index", "Home"); // Redireciona para a home após login bem-sucedido
                    }

                    TempData["MensagemErro"] = "Usuário ou senha inválidos. Por favor, tente novamente.";
                }
                else
                {
                    TempData["MensagemErro"] = "Por favor, verifique os dados inseridos.";
                }

                return View("Index", loginModel); // Retorna à tela de login caso ocorra algum erro
            }
            catch (Exception ex)
            {
                TempData["MensagemErro"] = $"Erro ao tentar realizar login: {ex.Message}";
                return RedirectToAction("Index");
            }
        }

        // Envia um link para redefinir a senha do usuário
        [HttpPost]
        public async Task<IActionResult> EnviarLinkParaRedefinirSenha(RedefinirSenhaModel redefinirSenhaModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var usuario = await _usuarioRepositorio.BuscarUsuarioExistenteAsync(redefinirSenhaModel.Email);

                    if (usuario != null)
                    {
                        string novaSenha = usuario.GerarNovaSenha();
                        string mensagem = $"Sua nova senha é {novaSenha}";

                        bool emailEnviado = _email.Enviar(usuario.Email, "DigitalStore - NovaSenha", mensagem);

                        if (emailEnviado)
                        {
                            await _usuarioRepositorio.AtualizarUsuarioAsync(usuario);
                            TempData["MensagemSucesso"] = "Enviamos para seu e-mail cadastrado uma nova senha.";
                        }
                        else
                        {
                            TempData["MensagemErro"] = "Não conseguimos enviar o e-mail. Por favor, tente novamente.";
                        }
                        return RedirectToAction("Index", "Login");
                    }

                    TempData["MensagemErro"] = "Usuário não encontrado. Tente novamente.";
                }

                return View("Index"); // Retorna ao login caso o modelo esteja inválido
            }
            catch (Exception ex)
            {
                TempData["MensagemErro"] = $"Erro ao enviar o e-mail: {ex.Message}";
                return RedirectToAction("Index");
            }
        }
    }
}