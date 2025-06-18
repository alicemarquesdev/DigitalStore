using DigitalStore.Models;

namespace DigitalStore.Repositorio.Interfaces
{
    // Interface que define os métodos para manipulação dos dados de usuários no banco de dados.
    public interface IUsuarioRepositorio
    {
        // Retorna um usuário existente com base no e-mail fornecido, usado na redefinição de senha e login do usuário.
        Task<UsuarioModel?> BuscarUsuarioExistenteAsync(string email);

        // Retorna um usuário com base no ID fornecido.
        Task<UsuarioModel?> BuscarUsuarioPorIdAsync(int id);

        // Adiciona um novo usuário ao banco de dados.
        Task AddUsuarioAsync(UsuarioModel usuario);

        // Atualiza os dados de um usuário existente.
        Task AtualizarUsuarioAsync(UsuarioModel usuario);

        // Remove um usuário com base no ID fornecido.
        Task<bool> RemoverUsuarioAsync(UsuarioModel usuario);
    }
}
