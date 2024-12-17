using DigitalStore.Models;

namespace DigitalStore.Repositorio.Interfaces
{
    public interface IUsuarioRepositorio
    {
        Task<UsuarioModel> BuscarUsuarioExistenteAsync(string email);

        Task<UsuarioModel> BuscarUsuarioPorIdAsync(int id);

        Task AddUsuarioAsync(UsuarioModel usuario);

        Task AtualizarUsuarioAsync(UsuarioModel usuario);

        Task AlterarSenhaAsync(AlterarSenhaModel alterarSenhaModel);

        Task<bool> RemoverUsuarioAsync(int id);
    }
}