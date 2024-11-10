using DigitalStore.Models;

namespace DigitalStore.Repositorio
{
    public interface IUsuarioRepositorio
    {
        Task<UsuarioModel> BuscarUsuarioPorIdAsync(int id);

        Task<UsuarioModel> BuscarUsuarioExistenteAsync(string email);

        Task AddUsuarioAsync(UsuarioModel usuario);

        Task AtualizarUsuarioAsync(UsuarioSemSenhaModel usuario);

        Task<bool> RemoverUsuarioAsync(int id);
    }
}