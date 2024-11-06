using DigitalStore.Models;

namespace DigitalStore.Repositorio
{
    public interface IUsuarioRepositorio
    {
        UsuarioModel ListarPorId(int id);
        UsuarioModel BuscarPorEmail(string email);

        UsuarioModel Criar(UsuarioModel usuario);
        UsuarioModel Atualizar(UsuarioModel usuario);

        bool Apagar(int id);

    }
}
