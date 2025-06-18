using DigitalStore.Filters;
using DigitalStore.Helper.Interfaces;
using DigitalStore.Models;
using DigitalStore.Repositorio.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace DigitalStore.Controllers
{
    // Controlador responsável pela gestão de usuários.
    // - MinhaConta(): Exibe os dados do usuário logado.
    // - EditarUsuario(UsuarioSemSenhaModel usuarioSemSenha): Atualiza os dados do usuário.
    // - RemoverUsuario(int id): Remove um usuário do sistema.
    [PaginaCliente]
    public class UsuarioController : Controller
    {
        // Declaração das dependências que serão injetadas no controlador.
        private readonly IUsuarioRepositorio _usuarioRepositorio;
        private readonly IPedidoRepositorio _pedidoRepositorio;
        private readonly ISessao _sessao;
        private readonly ILogger<UsuarioController> _logger;

        // Construtor para injeção de dependências
        public UsuarioController(IUsuarioRepositorio usuarioRepositorio, IPedidoRepositorio pedidoRepositorio, ISessao sessao, ILogger<UsuarioController> logger)
        {
            // Validação das dependências, caso algum parâmetro seja null, lança uma exceção.
            _usuarioRepositorio = usuarioRepositorio ?? throw new ArgumentNullException(nameof(usuarioRepositorio));
            _pedidoRepositorio = pedidoRepositorio;
            _sessao = sessao ?? throw new ArgumentNullException(nameof(sessao));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        // Exibe a página da conta do usuário logado
        public async Task<IActionResult> MinhaConta()
        {
            // Busca a sessão do usuário logado
            var usuarioSessao = _sessao.BuscarSessaoDoUsuario();

            // Se não encontrar o usuário na sessão, redireciona para a página de login
            if (usuarioSessao == null)
            {
                _logger.LogWarning("Usuário não encontrado na sessão.");
                TempData["Alerta"] = "Usuário não encontrado. Faça login novamente.";
                return RedirectToAction("Login", "Login");
            }

            // Busca os dados atualizados no banco
            var usuarioDb = await _usuarioRepositorio.BuscarUsuarioPorIdAsync(usuarioSessao.UsuarioId);
            if (usuarioDb == null)
            {
                _logger.LogWarning("Usuário não encontrado no banco após login.");
                TempData["Alerta"] = "Usuário não encontrado no banco.";
                return RedirectToAction("Login", "Login");
            }

            var usuarioSemSenha = new UsuarioSemSenhaModel
            {
                Id = usuarioDb.UsuarioId,
                Nome = usuarioDb.Nome,
                Email = usuarioDb.Email,
                DataNascimento = usuarioDb.DataNascimento,
                Genero = usuarioDb.Genero
            };

            return View(usuarioSemSenha);
        }

        // Método para editar os dados do usuário
        [HttpPost]
        public async Task<IActionResult> EditarUsuario(UsuarioSemSenhaModel usuarioSemSenha)
        {
            try
            {
                // Verifica se os dados do usuário foram passados
                if (usuarioSemSenha == null)
                {
                    throw new ArgumentNullException(nameof(usuarioSemSenha), "Os dados do usuário são inválidos.");
                }

                // Verifica se o modelo está válido
                if (!ModelState.IsValid)
                {
                    TempData["Alerta"] = "Os dados inseridos não são válidos.";
                    return View("MinhaConta", usuarioSemSenha); // Retorna para a tela de criação caso o e-mail já exista.
                }

                // Busca o usuário no banco de dados para atualizar
                var usuarioDb = await _usuarioRepositorio.BuscarUsuarioPorIdAsync(usuarioSemSenha.Id);

                // Se o usuário não for encontrado no banco de dados, lança uma exceção
                if (usuarioDb == null)
                {
                    throw new InvalidOperationException($"Usuário com ID {usuarioSemSenha.Id} não encontrado no banco de dados.");
                }
                var emailExistente = await _usuarioRepositorio.BuscarUsuarioExistenteAsync(usuarioSemSenha.Email);

                // Verifica se o e-mail já está cadastrado.
                if (emailExistente != null && emailExistente.UsuarioId != usuarioDb.UsuarioId)
                {
                    TempData["Alerta"] = "E-mail já cadastrado. Tente novamente com outro e-mail.";
                    return View("MinhaConta", usuarioSemSenha); // Retorna para a tela de criação caso o e-mail já exista.
                }

                // Cria um novo objeto de usuário com os dados atualizados
                var usuario = new UsuarioModel()
                {
                    UsuarioId = usuarioSemSenha.Id,
                    Nome = usuarioSemSenha.Nome,
                    Email = usuarioSemSenha.Email,
                    DataNascimento = usuarioSemSenha.DataNascimento,
                    Genero = usuarioSemSenha.Genero,
                    Perfil = usuarioDb.Perfil,
                    Senha = usuarioDb.Senha
                };

                // Atualiza o usuário no banco de dados
                await _usuarioRepositorio.AtualizarUsuarioAsync(usuario);

                // Exibe mensagem de sucesso
                TempData["Alerta"] = "Os dados do usuário foram atualizados com sucesso!";
                return RedirectToAction("MinhaConta");
            }
            catch (Exception ex)
            {
                // Em caso de erro, registra o erro e exibe mensagem para o usuário
                _logger.LogError(ex, "Erro ao atualizar os dados do usuário.");
                TempData["Alerta"] = "Ocorreu um erro ao atualizar os dados do usuário. Tente novamente mais tarde.";
                return View("MinhaConta", usuarioSemSenha); 
            }
        }

        // Método para remover um usuário
        [HttpPost]
        public async Task<IActionResult> RemoverUsuario(int id)
        {
            try
            {
                // Verifica se o ID do usuário é válido
                if (id <= 0)
                {
                    throw new ArgumentException("ID de usuário inválido.", nameof(id));
                }

                // Busca o usuário no banco de dados
                var usuario = await _usuarioRepositorio.BuscarUsuarioPorIdAsync(id);

                // Se o usuário não for encontrado, lança uma exceção
                if (usuario == null)
                {
                    throw new ArgumentException("Usuário não encontrado no banco de dados.");
                }

                // Busca todos os pedidos do usuário para verificar se ele pode ser removido
                var pedidosDoUsuario = await _pedidoRepositorio.BuscarTodosOsPedidosDoUsuarioAsync(id);

                // Verifica se algum pedido está em andamento
                foreach (var pedido in pedidosDoUsuario)
                {
                    // Se o pedido não estiver concluído ou cancelado, o usuário não pode ser excluído
                    if (pedido.StatusDoPedido != Enums.PedidoEnum.Concluido || pedido.StatusDoPedido != Enums.PedidoEnum.Cancelado)
                    {
                        _logger.LogError("O usuário não pode ser excluído, pedidos ainda estão em andamento.");
                        TempData["Alerta"] = "O usuário não pode ser excluído, pedidos ainda estão em andamento.";
                        return RedirectToAction("MinhaConta");
                    }
                }

                // Remove o usuário do banco de dados
                await _usuarioRepositorio.RemoverUsuarioAsync(usuario);

                // Exibe mensagem de sucesso
                TempData["Alerta"] = "Usuário removido com sucesso.";
                return RedirectToAction("Index", "Home");
            }
            catch (Exception ex)
            {
                // Registra erro e exibe mensagem de erro para o usuário
                _logger.LogError(ex, "Erro ao tentar remover o usuário com ID.");
                TempData["Alerta"] = "Não foi possível remover o usuário. Tente novamente mais tarde.";
                return RedirectToAction("MinhaConta");
            }
        }
    }
}
