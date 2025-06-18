using DigitalStore.Data;
using DigitalStore.Helper;
using DigitalStore.Models;
using DigitalStore.Repositorio.Interfaces;

namespace DigitalStore.Repositorio
{
    // Implementa métodos relacionados a alteração de senha do Usuário
    // - AlterarSenhaAsync(AlterarSenhaModel alterarSenhaModel)
    // - RedefinirSenhaAsync(int id, string novasenha)
    public class AlterarSenhaRepositorio : IAlteracaoSenhaRepositorio
    {
        private readonly IUsuarioRepositorio _usuarioRepositorio;
        private readonly BancoContext _context;
        private readonly ILogger<AlterarSenhaRepositorio> _logger;

        // Injeção de dependência do repositório de usuário e do contexto do banco de dados
        public AlterarSenhaRepositorio(IUsuarioRepositorio usuarioRepositorio,
                                        BancoContext context, 
                                        ILogger<AlterarSenhaRepositorio> logger)
        {
            _usuarioRepositorio = usuarioRepositorio;
            _context = context;
            _logger = logger;
        }

        // Método para alterar a senha do usuário, usando criptografia SHA1
        public async Task AlterarSenhaAsync(AlterarSenhaModel alterarSenhaModel)
        {
            try
            {
                // Usando o repositório de usuário para buscar o usuário
                var usuarioDB = await _usuarioRepositorio.BuscarUsuarioPorIdAsync(alterarSenhaModel.Id);

                // Verifica se o usuário existe
                if (usuarioDB == null)
                {
                    throw new KeyNotFoundException("Usuário não encontrado para atualização de senha");
                }

                // Verifica se a senha atual está correta
                if (!usuarioDB.SenhaValida(alterarSenhaModel.SenhaAtual))
                {
                    throw new InvalidOperationException("Senha atual não confere!");
                }

                // Verifica se a nova senha é diferente da senha atual
                if (usuarioDB.SenhaValida(alterarSenhaModel.NovaSenha))
                {
                    throw new InvalidOperationException("Nova senha deve ser diferente da senha atual!");
                }

                // Define a nova senha e criptografa
                usuarioDB.SetNovaSenha(alterarSenhaModel.NovaSenha);

                // Atualiza o usuário no banco de dados
                _context.Usuarios.Update(usuarioDB);

                // Tenta salvar as mudanças no banco de dados
                var result = await _context.SaveChangesAsync();

                // Verifica se o número de alterações é 0, o que indicaria que nada foi alterado
                if (result == 0)
                {
                    throw new Exception("Falha ao atualizar a senha do usuário.");
                }
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogError(ex, "Erro ao alterar senha.");
                throw new InvalidOperationException(ex.Message);
            }
            catch (Exception ex)
            {
                // Captura qualquer exceção e lança uma exceção genérica com a mensagem de erro
                _logger.LogError(ex, "Erro ao alterar senha.");
                throw new Exception("Ocorreu um erro ao alterar a senha.");
            }
        }

        // Método para redefinir a senha do usuário
        public async Task<bool> RedefinirSenhaAsync(int id, string novaSenha)
        {
            try
            {
                // Usando o repositório de usuário para buscar o usuário
                var usuarioDB = await _usuarioRepositorio.BuscarUsuarioPorIdAsync(id);

                // Verifica se o usuário existe
                if (usuarioDB == null)
                {
                    throw new KeyNotFoundException("Usuário não encontrado para redefinição de senha");
                }

                // Atribui a nova senha e criptografa
                usuarioDB.Senha = novaSenha.GerarHash();

                // Atualiza o usuário no banco de dados
                _context.Usuarios.Update(usuarioDB);

                // Tenta salvar as mudanças no banco de dados
                var result = await _context.SaveChangesAsync();

                // Verifica se o número de alterações é 0, o que indicaria que nada foi alterado
                if (result == 0)
                {
                    throw new InvalidOperationException("Falha ao redefinir a senha do usuário.");
                }

                // Retorna true se a senha foi redefinida com sucesso
                return true;
            }
            catch (Exception ex)
            {
                // Captura qualquer exceção e lança uma exceção genérica com a mensagem de erro
                _logger.LogError(ex, "Erro ao redefinir senha.");
                throw new Exception("Ocorreu um erro ao redefinir a senha.");
            }
        }
    }
}