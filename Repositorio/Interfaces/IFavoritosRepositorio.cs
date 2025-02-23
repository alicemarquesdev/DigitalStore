using DigitalStore.Models;

namespace DigitalStore.Repositorio.Interfaces
{
    public interface IFavoritosRepositorio
    {
        Task<List<FavoritosModel>> BuscarFavoritosDoUsuarioAsync(int usuarioId);

        Task<FavoritosModel> BuscarProdutoExistenteNoFavoritosAsync(int produtoId, int usuarioId);

        Task AddOuRemoverFavoritoAsync(int produtoId, int usuarioId);
    }
}