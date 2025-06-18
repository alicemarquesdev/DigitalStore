using DigitalStore.Filters;
using DigitalStore.Helper.Interfaces;
using DigitalStore.Models;
using DigitalStore.Repositorio.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace DigitalStore.Controllers
{
    /*
        Controlador responsável pela alteração de senha do usuário.
        
        Métodos:
        - AlterarSenha(): Exibe a página de alteração de senha.
        - (POST) AlterarSenha(AlterarSenhaModel alterarSenhaModel): Processa a alteração de senha.
    */
    public class AlterarSenhaController : Controller
    {
        // Injeção de dependências para repositório de alteração de senha, logger e sessão
        private readonly IAlteracaoSenhaRepositorio _alteracaoSenhaRepositorio;
        private readonly ILogger<AlterarSenhaController> _logger;
        private readonly ISessao _sessao;

        // Construtor para injeção de dependências
        public AlterarSenhaController(
                                IAlteracaoSenhaRepositorio alteracaoSenhaRepositorio,
                                ILogger<AlterarSenhaController> logger,
                                ISessao sessao)
        {
            // Garantia de que as dependências não sejam nulas
            _alteracaoSenhaRepositorio = alteracaoSenhaRepositorio ?? throw new ArgumentNullException(nameof(alteracaoSenhaRepositorio));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _sessao = sessao ?? throw new ArgumentNullException(nameof(sessao));
        }

        // Exibe a página de alteração de senha
        public IActionResult AlterarSenha()
        {
            try
            {
                // Recupera as informações da sessão do usuário
                var usuario = _sessao.BuscarSessaoDoUsuario();
                if (usuario == null)
                {
                    // Caso o usuário não esteja logado, redireciona para a tela de login
                    TempData["Alerta"] = "Sessão expirada. Faça login novamente.";
                    return RedirectToAction("Login", "Login");
                }

                // Preenche o modelo para a página de alteração de senha
                var alterarSenhaModel = new AlterarSenhaModel()
                {
                    Id = usuario.UsuarioId,  // Definindo o ID do usuário
                    SenhaAtual = string.Empty, // Senha atual estará vazia inicialmente
                    NovaSenha = string.Empty,  // Nova senha estará vazia inicialmente
                    ConfirmarNovaSenha = string.Empty  // Confirmar nova senha também vazia
                };

                return View(alterarSenhaModel);  // Exibe a view com o modelo
            }
            catch (Exception ex)
            {
                // Em caso de erro, registra o log e exibe uma mensagem para o usuário
                _logger.LogError(ex, "Erro ao acessar página de Alterar Senha");
                TempData["Alerta"] = "Houve um erro ao acessar a página de alteração de senha. Tente novamente.";
                return RedirectToAction("MinhaConta", "Usuario");
            }
        }

        // Método para processar a alteração de senha do usuário via POST
        [HttpPost]
        public async Task<IActionResult> AlterarSenha(AlterarSenhaModel alterarSenhaModel)
        {
            try
            {
                // Validação do modelo recebido
                if (alterarSenhaModel == null)
                {
                    throw new ArgumentNullException(nameof(alterarSenhaModel), "Os dados da alteração de senha são inválidos.");
                }

                // Verifica se o modelo é válido de acordo com as regras de validação
                if (!ModelState.IsValid)
                {
                    // Caso haja erro de validação, exibe mensagem e retorna para a página de alteração de senha
                    TempData["Alerta"] = "Os dados inseridos são inválidos. Verifique e tente novamente.";
                    return View("AlterarSenha", alterarSenhaModel);
                }

                // Chama o repositório para alterar a senha no banco de dados
                await _alteracaoSenhaRepositorio.AlterarSenhaAsync(alterarSenhaModel);
                // Exibe mensagem de sucesso e redireciona para a página "Minha Conta"
                TempData["Alerta"] = "Senha alterada com sucesso!";

                return RedirectToAction("MinhaConta", "Usuario");
            }
            catch (Exception ex)
            {
                // Registra o erro no log e exibe uma mensagem de erro para o usuário
                _logger.LogError(ex, "Erro ao atualizar a senha.");
                if (ex.InnerException is InvalidOperationException || ex is InvalidOperationException)
                {
                    TempData["Alerta"] = ex.Message;  // Exibe a mensagem amigável
                }
                else
                {
                    TempData["Alerta"] = "Erro ao alterar a senha. Por favor, tente novamente mais tarde.";
                }
                return View("AlterarSenha", alterarSenhaModel);  // Retorna para a página de alteração de senha
            }
        }
    }
}
