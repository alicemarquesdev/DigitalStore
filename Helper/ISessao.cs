using DigitalStore.Models;

namespace DigitalStore.Helper
{
    public interface ISessao
    {
        UsuarioModel BuscarSessaoDoUsuario();

        void CriarSessaoDoUsuario(UsuarioModel usuario);

        void RemoverSessaoDoUsuario();
    }
}