using DigitalStore.Filters;
using DigitalStore.Helper;
using DigitalStore.Helper.Interfaces;
using DigitalStore.Models;
using DigitalStore.Repositorio.Interfaces;
using DigitalStore.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace DigitalStore.Controllers
{
    /* 
        Controller responsável pela gestão dos endereços de um usuário.
        
        Métodos:
        - Enderecos(int id): Exibe a lista de endereços do usuário e um formulário para adicionar um novo endereço.
        - (POST) AddEndereco(EnderecoModel endereco): Adiciona um novo endereço para o usuário, com verificações de validade.
        - (POST) RemoverEndereco(int id): Remove um endereço baseado no ID do endereço.
    */
    [PaginaCliente]
    public class EnderecoController : Controller
    {
        // Declaração das dependências injetadas no controlador
        private readonly string _googleApiKey;
        private readonly IEnderecoRepositorio _enderecoRepositorio;
        private readonly ISessao _sessao;
        private readonly ILogger<EnderecoController> _logger;  // ILogger para registrar logs de erro e sucesso.

        // Construtor do controlador. Recebe o repositório de endereços e o ILogger como dependências.
        // Adiciona checagem para garantir que as dependências não sejam nulas.
        public EnderecoController(IOptions<GoogleAPISettings> googleApiKey, IEnderecoRepositorio enderecoRepositorio, ISessao sessao, ILogger<EnderecoController> logger)
        {
            _googleApiKey = googleApiKey.Value.ApiKey ?? throw new ArgumentNullException(nameof(googleApiKey.Value.ApiKey));
            _enderecoRepositorio = enderecoRepositorio ?? throw new ArgumentNullException(nameof(enderecoRepositorio));
            _sessao = sessao ?? throw new ArgumentNullException(nameof(sessao));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        // Método GET para exibir os endereços do usuário e o formulário de adicionar um novo endereço.
        [HttpGet]
        public async Task<IActionResult> Enderecos()
        {
            try
            {
                // Busca a sessão do usuário logado.
                var usuario = _sessao.BuscarSessaoDoUsuario();
                if (usuario == null)
                {
                    throw new ArgumentException("Sessao do usuário é nula");
                }

                // Busca todos os endereços do usuário a partir do repositório.
                var enderecos = await _enderecoRepositorio.BuscarTodosOsEnderecosDoUsuarioAsync(usuario.UsuarioId);

                // Criação de um modelo de endereço vazio para preenchimento na view.
                var endereco = new EnderecoModel
                {
                    UsuarioId = usuario.UsuarioId,
                    EnderecoCompleto = string.Empty
                };

                // Criação do ViewModel com os dados do usuário e os endereços obtidos.
                var viewModel = new EnderecoViewModel
                {
                    GoogleApiKey = _googleApiKey,
                    UsuarioId = usuario.UsuarioId,
                    Enderecos = enderecos,
                    Endereco = endereco
                };

                return View(viewModel); // Retorna a view com o ViewModel contendo os dados
            }
            catch (Exception ex)
            {
                // Lida com qualquer outra exceção inesperada.
                _logger.LogError(ex, "Erro ao acessar os endereços do usuário.");
                TempData["Alerta"] = "Ocorreu um erro ao tentar acessar seus endereços. Tente novamente mais tarde.";
                return RedirectToAction("Index", "Home");
            }
        }

        // Método POST para adicionar um novo endereço.
        [HttpPost]
        public async Task<IActionResult> AddEndereco(EnderecoModel endereco)
        {
            try
            {
                // Valida o modelo de dados do endereço.
                if (ModelState.IsValid)
                {
                    // Verifica se o número de endereços já cadastrados pelo usuário não excede o limite.
                    var enderecosLimite = await _enderecoRepositorio.BuscarTodosOsEnderecosDoUsuarioAsync(endereco.UsuarioId);
                    if (enderecosLimite.Count >= 4)  // Limite de endereços por usuário
                    {
                        TempData["Alerta"] = "Você só pode ter no máximo 4 endereços cadastrados.";
                        return Redirect(Request.Headers["Referer"].ToString()); // Redireciona para a página anterior
                    }

                    // Adiciona o novo endereço ao repositório.
                    await _enderecoRepositorio.AddEnderecoAsync(endereco);
                    _logger.LogInformation($"Endereço adicionado com sucesso para o usuário {endereco.UsuarioId}."); // Loga a operação de sucesso
                    TempData["Alerta"] = "Endereço adicionado com sucesso!";
                    return Redirect(Request.Headers["Referer"].ToString()); // Redireciona para a página anterior
                }

                // Caso o modelo não seja válido, registra o erro de validação e redireciona para a página anterior.
                TempData["Alerta"] = "Houve um erro ao adicionar o endereço, verifique os dados inseridos.";
                return Redirect(Request.Headers["Referer"].ToString());
            }
            catch (Exception ex)
            {
                // Captura exceções inesperadas e registra o erro.
                _logger.LogError(ex, "Erro ao adicionar o endereço.");
                TempData["Alerta"] = "Ocorreu um erro ao tentar adicionar o endereço, tente novamente.";
                return Redirect(Request.Headers["Referer"].ToString()); // Redireciona para a página anterior
            }
        }

        // Método POST para remover um endereço.
        [HttpPost]
        public async Task<IActionResult> RemoverEndereco(int id)
        {
            try
            {
                // Valida se o ID do endereço é válido. Se não for, lança uma exceção.
                if (id <= 0)
                {
                    throw new ArgumentException("ID do endereço inválido.");
                }

                // Remove o endereço do repositório, caso o ID seja válido.
                await _enderecoRepositorio.RemoverEnderecoAsync(id);
                _logger.LogInformation($"Endereço {id} removido com sucesso."); // Loga a operação de sucesso
                TempData["Alerta"] = "Endereço removido com sucesso!";
                return Redirect(Request.Headers["Referer"].ToString()); // Redireciona para a página anterior após remoção
            }
            catch (Exception ex)
            {
                // Captura exceções inesperadas e registra o erro.
                _logger.LogError(ex, "Erro ao remover o endereço.");
                TempData["Alerta"] = "Ocorreu um erro ao tentar remover o endereço.";
                return Redirect(Request.Headers["Referer"].ToString()); // Redireciona para a página anterior após erro
            }
        }
    }
}
