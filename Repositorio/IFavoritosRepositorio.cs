using DigitalStore.Models;

namespace DigitalStore.Repositorio
{
    public interface IFavoritosRepositorio
    {
        Task<List<FavoritosModel>> BuscarFavoritosDoUsuarioAsync(int usuarioId);

        Task<FavoritosModel> BuscarProdutoExistenteNoFavoritosAsync(int produtoId, int usuarioId);

        Task AddFavoritoAsync(int produtoId, int usuarioId);

        Task<bool> RemoverFavoritoAsync(int produtoId, int usuarioId);
    }
}