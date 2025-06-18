using DigitalStore.Helper.Interfaces;
using DigitalStore.Models;
using DigitalStore.Repositorio.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace DigitalStore.Controllers
{
    // Controlador responsável pela gestão de login e criação de usuário no sistema.
    // - Login(): Realiza o login do usuário.
    // - CriarUsuario(): Cria um novo usuário no sistema.
    // - RedefinirSenha(): Exibe a tela para redefinir a senha do usuário.
    // - Sair(): Realiza o logout e remove a sessão do usuário.
    // - CriarUsuario (POST): Cria um novo usuário no sistema e envia um e-mail de boas-vindas.
    // - Entrar (POST): Realiza o login do usuário, criando a sessão se os dados de login forem válidos.
    // - EnviarLinkParaRedefinirSenha (POST): Envia um link para redefinir a senha do usuário, caso o e-mail informado esteja cadastrado.
    public class LoginController : Controller
    {
        // Declaração das dependências dos repositórios e dos serviços.
        private readonly IAlteracaoSenhaRepositorio _alteracaoSenhaRepositorio;
        private readonly IUsuarioRepositorio _usuarioRepositorio;
        private readonly IEmail _email;
        private readonly ISessao _sessao;
        private readonly ILogger<LoginController> _logger; // Logger para logar erros e eventos.

        // Construtor com injeção de dependência
        public LoginController(IAlteracaoSenhaRepositorio alteracaoSenhaRepositorio,
                               IUsuarioRepositorio usuarioRepositorio,
                               IEmail email,
                               ISessao sessao,
                               ILogger<LoginController> logger)
        {
            _alteracaoSenhaRepositorio = alteracaoSenhaRepositorio ?? throw new ArgumentNullException(nameof(alteracaoSenhaRepositorio));
            _usuarioRepositorio = usuarioRepositorio ?? throw new ArgumentNullException(nameof(usuarioRepositorio));
            _email = email ?? throw new ArgumentNullException(nameof(email));
            _sessao = sessao ?? throw new ArgumentNullException(nameof(sessao));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        // Método que exibe a página de criação de usuário.
        public IActionResult CriarUsuario()
        {
            var sessao = _sessao.BuscarSessaoDoUsuario();

            // Se o usuário já estiver logado, redireciona para a página inicial.
            if (sessao != null)
                return RedirectToAction("Index", "Home");
            return View(); // Retorna a tela de criação de usuário.
        }

        // Ação principal para exibir a tela de login.
        public IActionResult Login()
        {
            var sessao = _sessao.BuscarSessaoDoUsuario();

            // Se o usuário já estiver logado, redireciona para a página inicial.
            if (sessao != null)
                return RedirectToAction("Index", "Home");

            return View(); // Exibe a tela de login.
        }

        // Método que exibe a tela para redefinir a senha.
        public IActionResult RedefinirSenha()
        {
            var sessao = _sessao.BuscarSessaoDoUsuario();

            // Se o usuário já estiver logado, redireciona para a página inicial.
            if (sessao != null)
                return RedirectToAction("Index", "Home");

            return View(); // Exibe a tela para redefinir a senha.
        }

        // Método de logout que remove a sessão do usuário.
        public IActionResult Sair()
        {
            _sessao.RemoverSessaoDoUsuario(); // Remove a sessão do usuário.
            return RedirectToAction("Login", "Login"); // Redireciona para a tela de login.
        }

        // Cria um novo usuário no sistema.
        [HttpPost]
        public async Task<IActionResult> CriarUsuario(UsuarioModel usuario)
        {
            try
            {
                // Verifica se os dados do usuário não são nulos.
                if (usuario == null)
                    throw new ArgumentNullException(nameof(usuario), "Os dados do usuário não podem ser nulos.");

                if (ModelState.IsValid)
                {
                    var emailExistente = await _usuarioRepositorio.BuscarUsuarioExistenteAsync(usuario.Email);
                    // Verifica se o e-mail já está cadastrado.
                    if (emailExistente != null)
                    {
                        TempData["Alerta"] = "E-mail já cadastrado. Tente novamente com outro e-mail.";
                        return View(usuario); // Retorna para a tela de criação caso o e-mail já exista.
                    }

                    // Cria o usuário no repositório e envia um e-mail de boas-vindas.
                    await _usuarioRepositorio.AddUsuarioAsync(usuario);
                    TempData["Alerta"] = "O usuário foi criado com sucesso!";
                    var mensagem = "Conta criada com sucesso! Explore nossa loja e aproveite.";
                    await _email.EnviarEmailAsync(usuario.Email, "Bem-Vindo! - DigitalStore", mensagem);

                    return RedirectToAction("Login", "Login"); // Redireciona para a tela de login após criar o usuário.
                }

                return View(usuario); // Retorna para a tela de criação caso os dados sejam inválidos.
            }
            catch (Exception ex)
            {
                // Caso ocorra um erro ao tentar criar o usuário, loga o erro e exibe uma mensagem.
                _logger.LogError(ex, "Erro inesperado ao tentar criar o usuário.");
                TempData["Alerta"] = "Erro inesperado ao criar o usuário. Tente novamente mais tarde.";
                return RedirectToAction("CriarUsuario"); // Redireciona de volta para a criação de usuário.
            }
        }

        // Realiza o login do usuário.
        [HttpPost]
        public async Task<IActionResult> Entrar(LoginModel loginModel)
        {
            try
            {
                // Verifica se o modelo de login é válido.
                if (ModelState.IsValid)
                {
                    // Tenta buscar o usuário com o e-mail fornecido.
                    var usuario = await _usuarioRepositorio.BuscarUsuarioExistenteAsync(loginModel.Email);

                    // Verifica se o usuário existe e se a senha é válida.
                    if (usuario != null && usuario.SenhaValida(loginModel.Senha))
                    {
                        _sessao.CriarSessaoDoUsuario(usuario); // Cria a sessão do usuário.
                        return RedirectToAction("Index", "Home"); // Redireciona para a página inicial após o login.
                    }

                    TempData["Alerta"] = "Usuário ou senha inválidos. Por favor, tente novamente."; // Exibe um alerta caso os dados estejam incorretos.
                }
                else
                {
                    TempData["Alerta"] = "Por favor, verifique os dados inseridos."; // Caso o modelo de login seja inválido.
                }

                return View("Login", loginModel); // Retorna à tela de login com o modelo preenchido.
            }
            catch (Exception ex)
            {
                // Em caso de erro ao tentar realizar o login, registra o erro e exibe uma mensagem.
                _logger.LogError(ex, "Erro ao tentar realizar login: {Message}", ex.Message);
                TempData["Alerta"] = $"Erro ao tentar realizar login, tente novamente.";
                return RedirectToAction("Login"); // Redireciona de volta para a tela de login.
            }
        }

        // Envia um link para redefinir a senha do usuário.
        [HttpPost]
        public async Task<IActionResult> EnviarLinkParaRedefinirSenha(RedefinirSenhaModel redefinirSenhaModel)
        {
            try
            {
                // Verifica se o modelo de redefinição de senha é válido.
                if (redefinirSenhaModel == null)
                    throw new ArgumentNullException(nameof(redefinirSenhaModel), "Os dados para redefinir a senha não podem ser nulos.");

                if (ModelState.IsValid)
                {
                    // Busca o usuário no banco de dados.
                    var usuarioDb = await _usuarioRepositorio.BuscarUsuarioExistenteAsync(redefinirSenhaModel.Email);

                    // Se o usuário for encontrado, gera uma nova senha.
                    if (usuarioDb != null)
                    {
                        string novaSenha = usuarioDb.GerarNovaSenha();

                        string mensagem = $"Olá {usuarioDb.Nome},<br><br>Sua nova senha é: <strong>{novaSenha}</strong><br><br>Altere sua senha assim que possível.";

                        // Envia o e-mail com a nova senha.
                        var emailEnviado = await _email.EnviarEmailAsync(usuarioDb.Email, "Redefinição de Senha - DigitalStore", mensagem);

                        if (emailEnviado)
                        {
                            // Atualiza a senha no banco de dados.
                            var senhaAlterada = await _alteracaoSenhaRepositorio.RedefinirSenhaAsync(usuarioDb.UsuarioId, novaSenha);

                            if (senhaAlterada)
                            {
                                TempData["Alerta"] = "Enviamos uma nova senha para o seu e-mail cadastrado. Verifique sua caixa de entrada e também o spam.";
                                return RedirectToAction("Login", "Login"); // Redireciona para a tela de login após a redefinição.
                            }
                        }
                        else
                        {
                            TempData["Alerta"] = "Não conseguimos enviar o e-mail. Por favor, tente novamente."; // Caso o e-mail não seja enviado.
                        }
                        return RedirectToAction("RedefinirSenha", "Login");
                    }

                    TempData["Alerta"] = "Usuário não encontrado. Tente novamente."; // Caso o usuário não seja encontrado.
                }

                return View("RedefinirSenha"); // Retorna à tela de redefinição de senha caso o modelo esteja inválido.
            }
            catch (Exception ex)
            {
                // Caso ocorra um erro ao enviar o e-mail, loga o erro e exibe uma mensagem.
                _logger.LogError(ex, "Erro ao enviar o e-mail para redefinir senha.");
                TempData["Alerta"] = $"Erro ao enviar o e-mail: {ex.Message}";
                return RedirectToAction("RedefinirSenha"); // Retorna à tela de redefinição de senha.
            }
        }
    }
}
