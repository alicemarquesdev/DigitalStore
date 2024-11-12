using DigitalStore.Models;

namespace DigitalStore.Repositorio
{
    public interface IUsuarioRepositorio
    {
        Task AddUsuarioAsync(UsuarioModel usuario);

        Task<UsuarioModel> AlterarSenhaAsync(AlterarSenhaModel alterarSenhaModel);

        Task AtualizarUsuarioAsync(UsuarioModel usuario);

        Task<UsuarioModel> BuscarUsuarioExistenteAsync(string email);

        Task<UsuarioModel> BuscarUsuarioPorIdAsync(int id);

        Task<bool> RemoverUsuarioAsync(int id);
    }
}