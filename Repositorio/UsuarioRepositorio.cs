using DigitalStore.Data;
using DigitalStore.Models;
using DigitalStore.Repositorio.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DigitalStore.Repositorio
{
    // Classe responsável por acessar e manipular os dados do usuário no banco de dados.
    // - BuscarUsuarioExistenteAsync(string email) - Busca um usuário existente com base no e-mail fornecido.
    // - BuscarUsuarioPorIdAsync(int id) - Busca um usuário com base no ID fornecido.
    // - AddUsuarioAsync(UsuarioModel usuario) - Adiciona um novo usuário ao banco de dados.
    // - AtualizarUsuarioAsync(UsuarioModel usuario) - Atualiza os dados de um usuário existente.
    // - RemoverUsuarioAsync(UsuarioModel usuario) - Remove um usuário do banco de dados.
    public class UsuarioRepositorio : IUsuarioRepositorio
    {
        private readonly BancoContext _context;
        private readonly ILogger<UsuarioRepositorio> _logger;

        // Construtor que recebe o contexto do banco de dados
        public UsuarioRepositorio(BancoContext context, ILogger<UsuarioRepositorio> logger)
        {
            _context = context;
            _logger = logger;
        }

        // Método para buscar usuário pelo email 
        public async Task<UsuarioModel?> BuscarUsuarioExistenteAsync(string email)
        {
            try
            {
                // Busca um usuário com o email fornecido, sem diferenciar maiúsculas e minúsculas
                return await _context.Usuarios
                             .FirstOrDefaultAsync(x => x.Email.ToLower() == email.ToLower());

            }
            catch (Exception ex)
            {
                // Em caso de erro, registra a exceção e retorna null
                _logger.LogError(ex, "Erro ao buscar usuário por email: {Email}", email);
                throw new Exception("Erro ao buscar o usuário por email.");
            }
        }

        // Método para buscar usuário por ID
        public async Task<UsuarioModel?> BuscarUsuarioPorIdAsync(int id)
        {
            try
            {
                // Busca o usuário pelo ID fornecido
                return await _context.Usuarios
                                     .FirstOrDefaultAsync(x => x.UsuarioId == id);
            }
            catch (Exception ex)
            {
                // Em caso de erro, registra a exceção
                _logger.LogError(ex, "Erro ao buscar usuário por ID: {UsuarioId}", id);
                throw new Exception("Erro ao buscar o usuário.");
            }
        }

        // Método para adicionar um novo usuário
        public async Task AddUsuarioAsync(UsuarioModel usuario)
        {
            try
            {
                // Define a senha criptografada antes de adicionar
                usuario.SetSenhaHash();
                usuario.Nome = usuario.Nome.Trim();
                usuario.Email = usuario.Email.ToLower().Trim();

                // Adiciona o novo usuário ao banco de dados
                _context.Usuarios.Add(usuario);
                var result = await _context.SaveChangesAsync();

                if (result == 0)
                {
                    throw new Exception("Nenhuma alteração feita no banco de dados.");
                }
            }
            catch (Exception ex)
            {
                // Em caso de erro, registra a exceção
                _logger.LogError(ex, "Erro ao adicionar novo usuário: {Usuario}", usuario);
                throw new Exception("Erro ao adicionar novo usuário.");
            }
        }

        // Método para atualizar os dados do usuário
        public async Task AtualizarUsuarioAsync(UsuarioModel usuario)
        {
            try
            {
                // Busca o usuário existente
                var usuarioDb = await BuscarUsuarioPorIdAsync(usuario.UsuarioId);

                // Se o usuário não for encontrado, lança uma exceção
                if (usuarioDb == null)
                {
                    throw new KeyNotFoundException("Usuário não encontrado para atualização");
                }

                // Atualiza os dados do usuário no banco
                usuarioDb.Nome = usuario.Nome.Trim();
                usuarioDb.Email = usuario.Email.Trim().ToLower();
                usuarioDb.DataNascimento = usuario.DataNascimento;
                usuarioDb.Genero = usuario.Genero;

                _context.Usuarios.Update(usuarioDb);
                var result = await _context.SaveChangesAsync();

                if (result == 0)
                {
                    throw new Exception("Nenhuma alteração feita no banco de dados.");
                }
            }
            catch (Exception ex)
            {
                // Em caso de erro, registra a exceção
                _logger.LogError(ex, "Erro ao atualizar usuário: {Usuario}", usuario);
                throw new Exception("Erro ao atualizar usuário.");
            }
        }

        // Método para remover um usuário
        public async Task<bool> RemoverUsuarioAsync(UsuarioModel usuario)
        {
            try
            {
            
                // Remove o usuário do banco de dados
                _context.Usuarios.Remove(usuario);
                var result = await _context.SaveChangesAsync();

                return result > 0;
            }
            catch (Exception ex)
            {
                // Em caso de erro, registra a exceção
                _logger.LogError(ex, "Erro ao remover usuário: {Usuario}", usuario);
                throw new Exception("Erro ao remover usuário.");
            }
        }
    }
}
